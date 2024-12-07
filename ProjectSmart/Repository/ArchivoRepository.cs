using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class ArchivoRepository : BaseRepository<Archivo>, IArchivoRepository
    {
        private readonly IConfiguration _configuration;

        public ArchivoRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Archivo>> AddArchivo(Archivo archivo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
            {
                { "cl_id", archivo.cl_cliente.cl_id },
                { "tr_id", archivo.tr_tramite.tr_id },
                { "frm_id", archivo.frm_id },
                { "ar_nombre", archivo.ar_nombre },
                { "ar_fecha", archivo.ar_fecha },
                { "ar_estado", archivo.ar_estado },
                { "ar_pos", archivo.ar_pos },
                { "pg_id", archivo.pg_id }
            };
                ResponseEntity<Archivo> result = await Add(archivo, "sp_RegistrarArchivo", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Archivo>> UpdateArchivo(Archivo archivo)
        {
            var parameters = new Dictionary<string, object>
            {
                { "ar_id", archivo.ar_id },
                { "ar_nombre", archivo.ar_nombre },
            };
            ResponseEntity<Archivo> result = await Update(archivo, "sp_ActualizarArchivo", parameters);
            return result;
        }


        public async Task<ResponseEntity<Archivo>> GetArchivoByFrmId(int pg_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@pg_id"] = pg_id;
                ResponseEntity<Archivo> result = await GetAllDataById("sp_ObtenerArchivosPor_Pg_Id", (SqlDataReader dr) =>
                //ResponseEntity<Archivo> result = await GetAllDataById("sp_ObtenerArchivosPorFrmId", (SqlDataReader dr) =>
                {
                    return new Archivo()
                    {
                        ar_id = Convert.ToInt32(dr["ar_id"].ToString()),
                        tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                        cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                        //frm_id = dr.IsDBNull(dr.GetOrdinal("frm_id")) ? 0 : dr.GetInt32(dr.GetOrdinal("frm_id")),
                        pg_id = Convert.ToInt32(dr["pg_id"].ToString()),
                        ar_nombre = dr["ar_nombre"].ToString(),
                        ar_fecha = Convert.ToDateTime(dr["ar_fecha"].ToString()),
                        ar_estado = Convert.ToInt32(dr["ar_estado"].ToString()),
                        ar_pos = Convert.ToInt32(dr["ar_pos"].ToString()),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Archivo>> DeleteArchivoByFrmId(int ar_id)
        {
            try 
            { 
                var parameters = new Dictionary<string, object>
                {
                    { "ar_id", ar_id }
                };
                ResponseEntity<Archivo> result = await DeleteEntity("sp_EliminarArchivoPorId", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Archivo>> AddArchivoPDFcaso1(Archivo archivo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", archivo.cl_cliente.cl_id },
                    { "tr_id", archivo.tr_tramite.tr_id },
                    { "frm_id", archivo.frm_id },
                    { "ar_nombre", archivo.ar_nombre },
                    { "ar_fecha", archivo.ar_fecha },
                    { "ar_estado", archivo.ar_estado },
                    { "ar_pos", archivo.ar_pos }
                };
                ResponseEntity<Archivo> result = await Add(archivo, "sp_RegistrarArchivo", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Archivo> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public bool AddOrUpdate(Archivo archivo)
        {
            return ExecProcedure<SqlConnection, SqlCommand, bool>("sp_GuardarActualizarArchivo", (cmd) =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@trId", archivo.tr_tramite.tr_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@clId", archivo.cl_cliente.cl_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@frmId", archivo.frm_id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@arNombre", archivo.ar_nombre));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@arPos", archivo.ar_pos));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@arFecha", archivo.ar_fecha));

                return cmd.ExecuteNonQuery() > 0;
            });
        }
    }
}
