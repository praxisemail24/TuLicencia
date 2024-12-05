using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class DuplicadoLicRepository : BaseRepository<DuplicadoLic>, IDuplicadoLicRepository
    {
        private readonly IConfiguration _configuration;

        public DuplicadoLicRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<DuplicadoLic>> GetAllDuplicadoLic()
        {
            try
            {
            
            ResponseEntity<DuplicadoLic> result = await GetAllData("sp_ObtenerDuplicadoLic", (SqlDataReader dr) =>
            {
                return new DuplicadoLic()
                {
                    fdl_id = Convert.ToInt32(dr["fdl_id"].ToString()),
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    fdl_fecha = Convert.ToDateTime(dr["fdl_fecha"].ToString()),
                    fdl_estado = Convert.ToInt32(dr["fdl_estado"].ToString()),
                    fdl_tipoLicencia = dr["fdl_tipoLicencia"].ToString(),
                    fdl_numeroLicencia = dr["fdl_numeroLicencia"].ToString(),
                    fdl_categoria = dr["fdl_categoria"].ToString(),
                    fdl_vehiculoPesado = dr["fdl_vehiculoPesado"].ToString(),
                    fdl_identificacion = dr["fdl_identificacion"].ToString(),
                    fdl_numero = dr["fdl_numero"].ToString(),
                    fdl_StatusLegal = dr["fdl_statusLegal"].ToString(),
                    fdl_genero = dr["fdl_genero"].ToString(),
                    fdl_donante = dr["fdl_donante"].ToString(),
                    fdl_tipoSangre = dr["fdl_tipoSangre"].ToString(),
                    fdl_talla = dr["fdl_talla"].ToString(),
                    fdl_peso = dr["fdl_peso"].ToString(),
                    fdl_tez = dr["fdl_tez"].ToString(),
                    fdl_colorPelo = dr["fdl_colorPelo"].ToString(),
                    fdl_colorOjo = dr["fdl_colorOjo"].ToString(),
                    fdl_direccion = dr["fdl_direccion"].ToString(),
                    fdl_numeroDireccion = dr["fdl_numeroDireccion"].ToString(),
                    fdl_pueblo = dr["fdl_pueblo"].ToString(),
                    fdl_codigoPostal = dr["fdl_codigoPostal"].ToString(),
                    fdl_barrio = dr["fdl_barrio"].ToString(),
                    fdl_apartado = dr["fdl_apartado"].ToString(),
                    fdl_pueblo2 = dr["fdl_pueblo2"].ToString(),
                    fdl_codigoPostal2 = dr["fdl_codigoPostal2"].ToString(),
                    fdl_licenciaSuspendida = dr["fdl_licenciaSuspendida"].ToString(),
                    fdl_motivoSuspension = dr["fdl_motivoSuspension"].ToString(),
                    fdl_recluido = dr["fdl_recluido"].ToString(),
                    fdl_convictoBebida = dr["fdl_ConvictoBebida"].ToString(),
                    fdl_fechaConvictoBebida = Convert.ToDateTime(dr["fdl_fechaConvictoBebida"].ToString()),
                    fdl_convictoNarcotico = dr["fdl_convictoNarcotico"].ToString(),
                    fdl_fechaConvictoNarcotico = Convert.ToDateTime(dr["fdl_fechaConvictoNarcotico"].ToString()),
                    fdl_obligacionAlimentaria = dr["fdl_obligacionAlimentaria"].ToString(),
                    fdl_deudaAcca = dr["fdl_deudaAcca"].ToString(),
                    fdl_paisProcede = dr.IsDBNull(dr.GetOrdinal("fdl_paisProcede")) ? "" : dr.GetString(dr.GetOrdinal("fdl_paisProcede")),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<DuplicadoLic>> GetDuplicadoLicById(int fdl_id)
        {
            try
            {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@fdl_id"] = fdl_id;
            ResponseEntity<DuplicadoLic> result = await GetData("sp_ObtenerDuplicadoLicPorId", parameters, (SqlDataReader dr) =>
            {
                return new DuplicadoLic()
                {
                    fdl_id = Convert.ToInt32(dr["fdl_id"].ToString()),
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    fdl_fecha = Convert.ToDateTime(dr["fdl_fecha"].ToString()),
                    fdl_estado = Convert.ToInt32(dr["fdl_estado"].ToString()),
                    fdl_tipoLicencia = dr["fdl_tipoLicencia"].ToString(),
                    fdl_numeroLicencia = dr["fdl_numeroLicencia"].ToString(),
                    fdl_categoria = dr["fdl_categoria"].ToString(),
                    fdl_vehiculoPesado = dr["fdl_vehiculoPesado"].ToString(),
                    fdl_identificacion = dr["fdl_identificacion"].ToString(),
                    fdl_numero = dr["fdl_numero"].ToString(),
                    fdl_StatusLegal = dr["fdl_statusLegal"].ToString(),
                    fdl_genero = dr["fdl_genero"].ToString(),
                    fdl_donante = dr["fdl_donante"].ToString(),
                    fdl_tipoSangre = dr["fdl_tipoSangre"].ToString(),
                    fdl_talla = dr["fdl_talla"].ToString(),
                    fdl_peso = dr["fdl_peso"].ToString(),
                    fdl_tez = dr["fdl_tez"].ToString(),
                    fdl_colorPelo = dr["fdl_colorPelo"].ToString(),
                    fdl_colorOjo = dr["fdl_colorOjo"].ToString(),
                    fdl_direccion = dr["fdl_direccion"].ToString(),
                    fdl_numeroDireccion = dr["fdl_numeroDireccion"].ToString(),
                    fdl_pueblo = dr["fdl_pueblo"].ToString(),
                    fdl_codigoPostal = dr["fdl_codigoPostal"].ToString(),
                    fdl_barrio = dr["fdl_barrio"].ToString(),
                    fdl_apartado = dr["fdl_apartado"].ToString(),
                    fdl_pueblo2 = dr["fdl_pueblo2"].ToString(),
                    fdl_codigoPostal2 = dr["fdl_codigoPostal2"].ToString(),
                    fdl_licenciaSuspendida = dr["fdl_licenciaSuspendida"].ToString(),
                    fdl_motivoSuspension = dr["fdl_motivoSuspension"].ToString(),
                    fdl_recluido = dr["fdl_recluido"].ToString(),
                    fdl_convictoBebida = dr["fdl_ConvictoBebida"].ToString(),
                    fdl_fechaConvictoBebida = Convert.ToDateTime(dr["fdl_fechaConvictoBebida"].ToString()),
                    fdl_convictoNarcotico = dr["fdl_convictoNarcotico"].ToString(),
                    fdl_fechaConvictoNarcotico = Convert.ToDateTime(dr["fdl_fechaConvictoNarcotico"].ToString()),
                    fdl_obligacionAlimentaria = dr["fdl_obligacionAlimentaria"].ToString(),
                    fdl_deudaAcca = dr["fdl_deudaAcca"].ToString(),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<DuplicadoLic>> AddDuplicadoLic(DuplicadoLic duplicadoLic)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", duplicadoLic.cl_cliente.cl_id},
                    { "pg_id", duplicadoLic.pg_pago.pg_id},
                    { "tr_id", duplicadoLic.tr_tramite.tr_id},
                    { "fdl_fecha", duplicadoLic.fdl_fecha },
                    { "fdl_estado", duplicadoLic.fdl_estado },
                    { "fdl_tipoLicencia", duplicadoLic.fdl_tipoLicencia },
                    { "fdl_numeroLicencia", duplicadoLic.fdl_numeroLicencia },
                    { "fdl_categoria", duplicadoLic.fdl_categoria },
                    { "fdl_vehiculoPesado", duplicadoLic.fdl_vehiculoPesado },
                    { "fdl_identificacion", duplicadoLic.fdl_identificacion },
                    { "fdl_numero", duplicadoLic.fdl_numero },
                    { "fdl_statusLegal", duplicadoLic.fdl_StatusLegal },
                    { "fdl_genero", duplicadoLic.fdl_genero},
                    { "fdl_donante", duplicadoLic.fdl_donante},
                    { "fdl_tipoSangre", duplicadoLic.fdl_tipoSangre },
                    { "fdl_talla", duplicadoLic.fdl_talla},
                    { "fdl_peso", duplicadoLic.fdl_peso },
                    { "fdl_tez", duplicadoLic.fdl_tez },
                    { "fdl_colorPelo", duplicadoLic.fdl_colorPelo },
                    { "fdl_colorOjo", duplicadoLic.fdl_colorOjo },
                    { "fdl_direccion", duplicadoLic.fdl_direccion },
                    { "fdl_numeroDireccion", duplicadoLic.fdl_numeroDireccion },
                    { "fdl_pueblo", duplicadoLic.fdl_pueblo },
                    { "fdl_codigoPostal", duplicadoLic.fdl_codigoPostal },
                    { "fdl_barrio", duplicadoLic.fdl_barrio },
                    { "fdl_apartado", duplicadoLic.fdl_apartado},
                    { "fdl_pueblo2", duplicadoLic.fdl_pueblo2},
                    { "fdl_codigoPostal2", duplicadoLic.fdl_codigoPostal2},
                    { "fdl_licenciaSuspendida", duplicadoLic.fdl_licenciaSuspendida},
                    { "fdl_motivoSuspension", duplicadoLic.fdl_motivoSuspension},
                    { "fdl_recluido", duplicadoLic.fdl_recluido},
                    { "fdl_convictoBebida", duplicadoLic.fdl_convictoBebida},
                    { "fdl_fechaConvictoBebida", duplicadoLic.fdl_fechaConvictoBebida == null ? DBNull.Value : duplicadoLic.fdl_fechaConvictoBebida},
                    { "fdl_convictoNarcotico", duplicadoLic.fdl_convictoNarcotico},
                    { "fdl_fechaConvictoNarcotico", duplicadoLic.fdl_fechaConvictoNarcotico == null ? DBNull.Value : duplicadoLic.fdl_fechaConvictoNarcotico},
                    { "fdl_obligacionAlimentaria", duplicadoLic.fdl_obligacionAlimentaria},
                    { "fdl_deudaAcca", duplicadoLic.fdl_deudaAcca},
                    { "fdl_paisProcede", duplicadoLic.fdl_paisProcede == null ? DBNull.Value : duplicadoLic.fdl_paisProcede},
                };
                ResponseEntity<DuplicadoLic> result = await Add(duplicadoLic, "sp_RegistrarDuplicadoLic", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<DuplicadoLic>> UpdateDuplicadoLic(DuplicadoLic duplicadoLic)
        {
            try
            { 
                var parameters = new Dictionary<string, object>
                {
                    { "fdl_id", duplicadoLic.fdl_id },
                    { "cl_id", duplicadoLic.cl_cliente.cl_id},
                    { "pg_id", duplicadoLic.pg_pago.pg_id},
                    { "tr_id", duplicadoLic.tr_tramite.tr_id},
                    { "fdl_fecha", duplicadoLic.fdl_fecha },
                    { "fdl_estado", duplicadoLic.fdl_estado },
                    { "fdl_tipoLicencia", duplicadoLic.fdl_tipoLicencia },
                    { "fdl_numeroLicencia", duplicadoLic.fdl_numeroLicencia },
                    { "fdl_categoria", duplicadoLic.fdl_categoria },
                    { "fdl_vehiculoPesado", duplicadoLic.fdl_vehiculoPesado },
                    { "fdl_identificacion", duplicadoLic.fdl_identificacion },
                    { "fdl_numero", duplicadoLic.fdl_numero },
                    { "fdl_statusLegal", duplicadoLic.fdl_StatusLegal },
                    { "fdl_genero", duplicadoLic.fdl_genero},
                    { "fdl_donante", duplicadoLic.fdl_donante},
                    { "fdl_tipoSangre", duplicadoLic.fdl_tipoSangre },
                    { "fdl_talla", duplicadoLic.fdl_talla},
                    { "fdl_peso", duplicadoLic.fdl_peso },
                    { "fdl_tez", duplicadoLic.fdl_tez },
                    { "fdl_colorPelo", duplicadoLic.fdl_colorPelo },
                    { "fdl_colorOjo", duplicadoLic.fdl_colorOjo },
                    { "fdl_direccion", duplicadoLic.fdl_direccion },
                    { "fdl_numeroDireccion", duplicadoLic.fdl_numeroDireccion },
                    { "fdl_pueblo", duplicadoLic.fdl_pueblo },
                    { "fdl_codigoPostal", duplicadoLic.fdl_codigoPostal },
                    { "fdl_barrio", duplicadoLic.fdl_barrio },
                    { "fdl_apartado", duplicadoLic.fdl_apartado},
                    { "fdl_pueblo2", duplicadoLic.fdl_pueblo2},
                    { "fdl_codigoPostal2", duplicadoLic.fdl_codigoPostal2},
                    { "fdl_licenciaSuspendida", duplicadoLic.fdl_licenciaSuspendida},
                    { "fdl_motivoSuspension", duplicadoLic.fdl_motivoSuspension},
                    { "fdl_recluido", duplicadoLic.fdl_recluido},
                    { "fdl_convictoBebida", duplicadoLic.fdl_convictoBebida},
                    { "fdl_fechaConvictoBebida", duplicadoLic.fdl_fechaConvictoBebida == null ? DBNull.Value : duplicadoLic.fdl_fechaConvictoBebida},
                    { "fdl_convictoNarcotico", duplicadoLic.fdl_convictoNarcotico},
                    { "fdl_fechaConvictoNarcotico", duplicadoLic.fdl_fechaConvictoNarcotico == null ? DBNull.Value : duplicadoLic.fdl_fechaConvictoNarcotico},
                    { "fdl_obligacionAlimentaria", duplicadoLic.fdl_obligacionAlimentaria},
                    { "fdl_deudaAcca", duplicadoLic.fdl_deudaAcca},
                    { "fdl_paisProcede", duplicadoLic.fdl_paisProcede == null ? DBNull.Value : duplicadoLic.fdl_paisProcede},
                };
                ResponseEntity<DuplicadoLic> result = await Update(duplicadoLic, "sp_ActualizarDuplicadoLic", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<DuplicadoLic>> DeleteDuplicadoLic(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "fdl_id", id }
                };
                ResponseEntity<DuplicadoLic> result = await DeleteEntity("sp_EliminaDuplicadoLic", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };
            }
            catch (Exception ex)
            {
                return new ResponseEntity<DuplicadoLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

    }
}
