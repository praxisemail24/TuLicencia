using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class LicenciaApreRepository : BaseRepository<LicenciaApre>, ILicenciaApreRepository
    {
        private readonly IConfiguration _configuration;

        public LicenciaApreRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<LicenciaApre>> GetAllLicenciaApre()
        {
            try
            {

            ResponseEntity<LicenciaApre> result = await GetAllData("sp_ObtenerLicenciaApre", (SqlDataReader dr) =>
            {
                return new LicenciaApre()
                {
                    fla_id = Convert.ToInt32(dr["fla_id"].ToString()),
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    fla_fecha = Convert.ToDateTime(dr["fla_fecha"].ToString()),
                    fla_estado = Convert.ToInt32(dr["fla_estado"].ToString()),
                    fla_tipoLicencia = dr["fla_tipoLicencia"].ToString(),
                    fla_identificacion = dr["fla_identificacion"].ToString(),
                    fla_numero = dr["fla_numero"].ToString(),
                    fla_statusLegal = dr["fla_statusLegal"].ToString(),
                    fla_genero = dr["fla_genero"].ToString(),
                    fla_donante = dr["fla_donante"].ToString(),
                    fla_tipoSangre = dr["fla_tipoSangre"].ToString(),
                    fla_talla = dr["fla_talla"].ToString(),
                    fla_peso = dr["fla_peso"].ToString(),
                    fla_tez = dr["fla_tez"].ToString(),
                    fla_colorPelo = dr["fla_colorPelo"].ToString(),
                    fla_colorOjo = dr["fla_colorOjo"].ToString(),
                    fla_nombrePadre = dr["fla_nombrePadre"].ToString(),
                    fla_nombreMadre = dr["fla_nombreMadre"].ToString(),
                    fla_direccion = dr["fla_direccion"].ToString(),
                    fla_numeroDireccion = dr["fla_numeroDireccion"].ToString(),
                    fla_pueblo = dr["fla_pueblo"].ToString(),
                    fla_codigoPostal = dr["fla_codigoPostal"].ToString(),
                    fla_barrio = dr["fla_barrio"].ToString(),
                    fla_apartado = dr["fla_apartado"].ToString(),
                    fla_pueblo2 = dr["fla_pueblo2"].ToString(),
                    fla_codigoPostal2 = dr["fla_codigoPostal2"].ToString(),
                    fla_licencia = dr["fla_licenciaSuspendida"].ToString(),
                    fla_procedeLicencia = dr["fla_licenciaSuspendida"].ToString(),
                    fla_licenciaSuspendida = dr["fla_licenciaSuspendida"].ToString(),
                    fla_motivoSuspendido = dr["fla_motivoSuspendido"].ToString(),
                    fla_recluido = dr["fla_recluido"].ToString(),
                    fla_convictoBebida = dr["fla_ConvictoBebida"].ToString(),
                    fla_fechaConvictoBebida = Convert.ToDateTime(dr["fla_fechaConvictoBebida"].ToString()),
                    fla_convictoNarcotico = dr["fla_convictoNarcotico"].ToString(),
                    fla_fechaConvictoNarcotico = Convert.ToDateTime(dr["fla_fechaConvictoNarcotico"].ToString()),
                    fla_obligacionAlimentaria = dr["fla_obligacionAlimentaria"].ToString(),
                    fla_deudaAcca = dr["fla_deudaAcca"].ToString(),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaApre>> GetLicenciaApreById(int fla_id)
        {
            try { 

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@fla_id"] = fla_id;
            ResponseEntity<LicenciaApre> result = await GetData("sp_ObtenerLicenciaAprePorId", parameters, (SqlDataReader dr) =>
            {
                return new LicenciaApre()
                {
                    fla_id = Convert.ToInt32(dr["fla_id"].ToString()),
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    fla_fecha = Convert.ToDateTime(dr["fla_fecha"].ToString()),
                    fla_estado = Convert.ToInt32(dr["fla_estado"].ToString()),
                    fla_tipoLicencia = dr["fla_tipoLicencia"].ToString(),
                    fla_identificacion = dr["fla_identificacion"].ToString(),
                    fla_numero = dr["fla_numero"].ToString(),
                    fla_statusLegal = dr["fla_statusLegal"].ToString(),
                    fla_genero = dr["fla_genero"].ToString(),
                    fla_donante = dr["fla_donante"].ToString(),
                    fla_tipoSangre = dr["fla_tipoSangre"].ToString(),
                    fla_talla = dr["fla_talla"].ToString(),
                    fla_peso = dr["fla_peso"].ToString(),
                    fla_tez = dr["fla_tez"].ToString(),
                    fla_colorPelo = dr["fla_colorPelo"].ToString(),
                    fla_colorOjo = dr["fla_colorOjo"].ToString(),
                    fla_nombrePadre = dr["fla_nombrePadre"].ToString(),
                    fla_nombreMadre = dr["fla_nombreMadre"].ToString(),
                    fla_direccion = dr["fla_direccion"].ToString(),
                    fla_numeroDireccion = dr["fla_numeroDireccion"].ToString(),
                    fla_pueblo = dr["fla_pueblo"].ToString(),
                    fla_codigoPostal = dr["fla_codigoPostal"].ToString(),
                    fla_barrio = dr["fla_barrio"].ToString(),
                    fla_apartado = dr["fla_apartado"].ToString(),
                    fla_pueblo2 = dr["fla_pueblo2"].ToString(),
                    fla_codigoPostal2 = dr["fla_codigoPostal2"].ToString(),
                    fla_licencia = dr["fla_licenciaSuspendida"].ToString(),
                    fla_procedeLicencia = dr["fla_licenciaSuspendida"].ToString(),
                    fla_licenciaSuspendida = dr["fla_licenciaSuspendida"].ToString(),
                    fla_motivoSuspendido = dr["fla_motivoSuspendido"].ToString(),
                    fla_recluido = dr["fla_recluido"].ToString(),
                    fla_convictoBebida = dr["fla_ConvictoBebida"].ToString(),
                    fla_fechaConvictoBebida = Convert.ToDateTime(dr["fla_fechaConvictoBebida"].ToString()),
                    fla_convictoNarcotico = dr["fla_convictoNarcotico"].ToString(),
                    fla_fechaConvictoNarcotico = Convert.ToDateTime(dr["fla_fechaConvictoNarcotico"].ToString()),
                    fla_obligacionAlimentaria = dr["fla_obligacionAlimentaria"].ToString(),
                    fla_deudaAcca = dr["fla_deudaAcca"].ToString(),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaApre>> AddLicenciaApre(LicenciaApre licenciaApre)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", licenciaApre.cl_cliente.cl_id},
                    { "pg_id", licenciaApre.pg_pago.pg_id},
                    { "tr_id", licenciaApre.tr_tramite.tr_id},
                    { "fla_fecha", licenciaApre.fla_fecha },
                    { "fla_estado", licenciaApre.fla_estado },
                    { "fla_tipoLicencia", licenciaApre.fla_tipoLicencia },
                    { "fla_identificacion", licenciaApre.fla_identificacion },
                    { "fla_numero", licenciaApre.fla_numero },
                    { "fla_statusLegal", licenciaApre.fla_statusLegal },
                    { "fla_genero", licenciaApre.fla_genero},
                    { "fla_donante", licenciaApre.fla_donante},
                    { "fla_tipoSangre", licenciaApre.fla_tipoSangre },
                    { "fla_talla", licenciaApre.fla_talla},
                    { "fla_peso", licenciaApre.fla_peso },
                    { "fla_tez", licenciaApre.fla_tez },
                    { "fla_colorPelo", licenciaApre.fla_colorPelo },
                    { "fla_colorOjo", licenciaApre.fla_colorOjo },
                    { "fla_nombrePadre", licenciaApre.fla_nombrePadre },
                    { "fla_nombreMadre", licenciaApre.fla_nombreMadre },
                    { "fla_direccion", licenciaApre.fla_direccion },
                    { "fla_numeroDireccion", licenciaApre.fla_numeroDireccion },
                    { "fla_pueblo", licenciaApre.fla_pueblo },
                    { "fla_codigoPostal", licenciaApre.fla_codigoPostal },
                    { "fla_barrio", licenciaApre.fla_barrio },
                    { "fla_apartado", licenciaApre.fla_apartado},
                    { "fla_pueblo2", licenciaApre.fla_pueblo2},
                    { "fla_codigoPostal2", licenciaApre.fla_codigoPostal2},
                    { "fla_licencia", licenciaApre.fla_licencia},
                    { "fla_procedeLicencia", licenciaApre.fla_procedeLicencia},
                    { "fla_licenciaSuspendida", licenciaApre.fla_licenciaSuspendida},
                    { "fla_motivoSuspendido", licenciaApre.fla_motivoSuspendido},
                    { "fla_recluido", licenciaApre.fla_recluido},
                    { "fla_convictoBebida", licenciaApre.fla_convictoBebida},
                    { "fla_fechaConvictoBebida", licenciaApre.fla_fechaConvictoBebida == null ? DBNull.Value : licenciaApre.fla_fechaConvictoBebida},
                    { "fla_convictoNarcotico", licenciaApre.fla_convictoNarcotico},
                    { "fla_fechaConvictoNarcotico", licenciaApre.fla_fechaConvictoNarcotico == null ? DBNull.Value : licenciaApre.fla_fechaConvictoNarcotico},
                    { "fla_obligacionAlimentaria", licenciaApre.fla_obligacionAlimentaria},
                    { "fla_deudaAcca", licenciaApre.fla_deudaAcca}
                };
                ResponseEntity<LicenciaApre> result = await Add(licenciaApre, "sp_RegistrarLicenciaApre", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaApre>> UpdateLicenciaApre(LicenciaApre licenciaApre)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "fla_id", licenciaApre.fla_id },
                    { "cl_id", licenciaApre.cl_cliente.cl_id},
                    { "pg_id", licenciaApre.pg_pago.pg_id},
                    { "tr_id", licenciaApre.tr_tramite.tr_id},
                    { "fla_fecha", licenciaApre.fla_fecha },
                    { "fla_estado", licenciaApre.fla_estado },
                    { "fla_tipoLicencia", licenciaApre.fla_tipoLicencia },
                    { "fla_identificacion", licenciaApre.fla_identificacion },
                    { "fla_numero", licenciaApre.fla_numero },
                    { "fla_statusLegal", licenciaApre.fla_statusLegal },
                    { "fla_genero", licenciaApre.fla_genero},
                    { "fla_donante", licenciaApre.fla_donante},
                    { "fla_tipoSangre", licenciaApre.fla_tipoSangre },
                    { "fla_talla", licenciaApre.fla_talla},
                    { "fla_peso", licenciaApre.fla_peso },
                    { "fla_tez", licenciaApre.fla_tez },
                    { "fla_colorPelo", licenciaApre.fla_colorPelo },
                    { "fla_colorOjo", licenciaApre.fla_colorOjo },
                    { "fla_nombrePadre", licenciaApre.fla_nombrePadre },
                    { "fla_nombreMadre", licenciaApre.fla_nombreMadre },
                    { "fla_direccion", licenciaApre.fla_direccion },
                    { "fla_numeroDireccion", licenciaApre.fla_numeroDireccion },
                    { "fla_pueblo", licenciaApre.fla_pueblo },
                    { "fla_codigoPostal", licenciaApre.fla_codigoPostal },
                    { "fla_barrio", licenciaApre.fla_barrio },
                    { "fla_apartado", licenciaApre.fla_apartado},
                    { "fla_pueblo2", licenciaApre.fla_pueblo2},
                    { "fla_codigoPostal2", licenciaApre.fla_codigoPostal2},
                    { "fla_licencia", licenciaApre.fla_licencia},
                    { "fla_procedeLicencia", licenciaApre.fla_procedeLicencia},
                    { "fla_licenciaSuspendida", licenciaApre.fla_licenciaSuspendida},
                    { "fla_motivoSuspendido", licenciaApre.fla_motivoSuspendido},
                    { "fla_recluido", licenciaApre.fla_recluido},
                    { "fla_convictoBebida", licenciaApre.fla_convictoBebida},
                    { "fla_fechaConvictoBebida", licenciaApre.fla_fechaConvictoBebida == null ? DBNull.Value : licenciaApre.fla_fechaConvictoBebida},
                    { "fla_convictoNarcotico", licenciaApre.fla_convictoNarcotico},
                    { "fla_fechaConvictoNarcotico", licenciaApre.fla_fechaConvictoNarcotico == null ? DBNull.Value : licenciaApre.fla_fechaConvictoNarcotico},
                    { "fla_obligacionAlimentaria", licenciaApre.fla_obligacionAlimentaria},
                    { "fla_deudaAcca", licenciaApre.fla_deudaAcca}
                };
                ResponseEntity<LicenciaApre> result = await Update(licenciaApre, "sp_ActualizarLicenciaApre", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaApre>> DeleteLicenciaApre(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "fla_id", id }
                };
                ResponseEntity<LicenciaApre> result = await DeleteEntity("sp_EliminaLicenciaApre", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaApre> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

    }
}
