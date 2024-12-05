using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class AsignacionRepository : BaseRepository<Asignacion>, IAsignacionRepository
    {
        private readonly IConfiguration _configuration;

        public AsignacionRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Asignacion>> AddAsignacion(Asignacion asignacion)
        {
            try
            {
                    var parameters = new Dictionary<string, object>
                {
                    { "asig_id", asignacion.asig_id },
                    { "frm_id", asignacion.frm_id.FrmID },
                    { "tr_id", asignacion.tr_id.tr_id },
                    { "adm_id1", asignacion.adm_id1.adm_id },
                    { "adm_id2", asignacion.adm_id2.adm_id },
                    { "asig_fecha", asignacion.asig_fecha },
                    { "asig_Activo", asignacion.asig_Activo },
                };
                    ResponseEntity<Asignacion> result = await Add(asignacion, "sp_RegistrarAsignacion", parameters);
                    return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Asignacion> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Asignacion> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Asignacion>> AddAsignacionDoctor(Asignacion asignacion)
        {
            try
            {
                    var parameters = new Dictionary<string, object>
                {
                    { "asig_id", asignacion.asig_id },
                    { "frm_id", asignacion.frm_id.FrmID },
                    { "tr_id", asignacion.tr_id.tr_id },
                    { "adm_id1", asignacion.adm_id1.adm_id },
                    { "adm_id2", asignacion.adm_id2.adm_id },
                    { "asig_fecha", asignacion.asig_fecha },
                    { "asig_Activo", asignacion.asig_Activo },
                };
                    ResponseEntity<Asignacion> result = await Add(asignacion, "sp_RegistrarAsignacionDoctor", parameters);
                    return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Asignacion> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Asignacion> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Asignacion>> GetDatosAsignacion(int frm_id, int tr_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@frm_id"] = frm_id;
                parameters["@tr_id"] = tr_id;
                ResponseEntity<Asignacion> result = await GetAllDataById("sp_ObtenerDatosAsignacion", (SqlDataReader dr) =>
                {
                    return new Asignacion()
                    {
                        asig_id = Convert.ToInt32(dr[nameof(Asignacion.asig_id)].ToString()),
                        frm_id = new FormularioDTO { FrmID = Convert.IsDBNull(dr["frm_id"]) ? 0 : Convert.ToInt32(dr["frm_id"]) },
                        tr_id = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                        adm_id1 = new Administrador
                        {
                            adm_id = Convert.IsDBNull(dr["adm_id1"]) ? 0 : Convert.ToInt32(dr["adm_id1"]),
                            adm_nombres = dr["adm_nombres1"].ToString()
                        },
                        adm_id2 = new Administrador
                        {
                            adm_id = Convert.IsDBNull(dr["adm_id2"]) ? 0 : Convert.ToInt32(dr["adm_id2"]),
                            adm_nombres = dr["adm_nombres2"].ToString()
                        },
                        asig_fecha = Convert.ToDateTime(dr[nameof(Asignacion.asig_fecha)]),
                        asig_Activo = Convert.ToBoolean(dr[nameof(Asignacion.asig_Activo)]),
                        statusactual = dr["statusactual"].ToString(),
                        motivoAnulacion = dr["motivoAnulacion"].ToString(),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Asignacion> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Asignacion> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<LineaTiempoTramite>> ObtenerLineaTiempo(int frm_id, int tr_id)
        {
            try
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@tramite_id", frm_id);
                parameters.Add("@tipo_id", tr_id);

                var r = await GetAllDataById<LineaTiempoTramite>("sp_ObtenerLinaTiempo", (reader) =>
                {
                    var linea = new LineaTiempoTramite();
                    linea.Paso = reader.IsDBNull(reader.GetOrdinal("paso")) ? null : reader.GetInt32(reader.GetOrdinal("paso"));
                    linea.Fecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"));
                    linea.Titulo = reader.IsDBNull(reader.GetOrdinal("titulo")) ? string.Empty : reader.GetString(reader.GetOrdinal("titulo"));
                    linea.Descripcion = reader.IsDBNull(reader.GetOrdinal("descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("descripcion"));
                    linea.ReferenciaId = reader.IsDBNull(reader.GetOrdinal("ref_id")) ? null : reader.GetInt32(reader.GetOrdinal("ref_id"));
                    linea.Referencia = reader.IsDBNull(reader.GetOrdinal("referencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("referencia"));
                    return linea;
                }, parameters);

                return r.items;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error en base de datos {sqlEx}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrio un error: {ex}");
                return null;
            }
        }
    }
}
