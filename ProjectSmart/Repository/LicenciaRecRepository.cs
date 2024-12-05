using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class LicenciaRecRepository : BaseRepository<LicenciaRec>, ILicenciaRecRepository
    {
        private readonly IConfiguration _configuration;

        public LicenciaRecRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<LicenciaRec>> GetAllLicenciaRec()
        {
            try
            {

            ResponseEntity<LicenciaRec> result = await GetAllData("sp_ObtenerLicenciaRec", (SqlDataReader dr) =>
            {
                return new LicenciaRec()
                {
                    flr_id = Convert.ToInt32(dr["flr_id"].ToString()),
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    flr_fecha = Convert.ToDateTime(dr["flr_fecha"].ToString()),
                    flr_estado = Convert.ToInt32(dr["flr_estado"].ToString()),
                    flr_tipoLicencia = dr["flr_tipoLicencia"].ToString(),
                    flr_numeroLicencia = dr["flr_numeroLicencia"].ToString(),
                    flr_categoria = dr["flr_categoria"].ToString(),
                    flr_tipoVehiculo = dr["flr_tipoVehiculo"].ToString(),
                    flr_paisProcede = dr["flr_paisProcede"].ToString(),
                    flr_estadoProcede = dr["flr_estadoProcede"].ToString(),
                    flr_numeroLicencia2 = dr["flr_numeroLicencia2"].ToString(),
                    flr_fechaExpiracion = Convert.ToDateTime(dr["flr_fechaExpiracion"].ToString()),
                    flr_identificacion = dr["flr_identificacion"].ToString(),
                    flr_numeroIdentificacion = dr["flr_numeroIdentificacion"].ToString(),
                    flr_statusLegal = dr["flr_statusLegal"].ToString(),
                    flr_genero = dr["flr_genero"].ToString(),
                    flr_donante = dr["flr_donante"].ToString(),
                    flr_tipoSangre = dr["flr_tipoSangre"].ToString(),
                    flr_talla = dr["flr_talla"].ToString(),
                    flr_peso = dr["flr_peso"].ToString(),
                    flr_tez = dr["flr_tez"].ToString(),
                    flr_colorPelo = dr["flr_colorPelo"].ToString(),
                    flr_colorOjo = dr["flr_colorOjo"].ToString(),
                    flr_nombrePadre = dr["flr_nombrePadre"].ToString(),
                    flr_nombreMadre = dr["flr_nombreMadre"].ToString(),
                    flr_direccion = dr["flr_direccion"].ToString(),
                    flr_numeroDireccion = dr["flr_numeroDireccion"].ToString(),
                    flr_pueblo = dr["flr_pueblo"].ToString(),
                    flr_codigoPostal = dr["flr_codigoPostal"].ToString(),
                    flr_barrio = dr["flr_barrio"].ToString(),
                    flr_apartado = dr["flr_apartado"].ToString(),
                    flr_pueblo2 = dr["flr_pueblo2"].ToString(),
                    flr_codigoPostal2 = dr["flr_codigoPostal2"].ToString(),
                    flr_licenciaSuspendida = dr["flr_licenciaSuspendida"].ToString(),
                    flr_motivoSuspencion = dr["flr_motivoSuspencion"].ToString(),
                    flr_numeroLicenciaPR = dr["flr_numeroLicenciaPR"].ToString(),
                    flr_recluido = dr["flr_recluido"].ToString(),
                    flr_convictoBebida = dr["flr_ConvictoBebida"].ToString(),
                    flr_fechaConvictoBebida = Convert.ToDateTime(dr["flr_fechaConvictoBebida"].ToString()),
                    flr_convictoNarcotico = dr["flr_convictoNarcotico"].ToString(),
                    flr_fechaConvictoNarcotico = Convert.ToDateTime(dr["flr_fechaConvictoNarcotico"].ToString()),
                    flr_obligacionAlimentaria = dr["flr_obligacionAlimentaria"].ToString(),
                    flr_deudaAcca = dr["flr_deudaAcca"].ToString(),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaRec>> GetLicenciaRecById(int id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@flr_id"] = id;
                ResponseEntity<LicenciaRec> result = await GetData("sp_ObtenerLicenciaRecPorId", parameters, (SqlDataReader dr) =>
                {
                    return new LicenciaRec()
                    {
                        flr_id = Convert.ToInt32(dr["flr_id"].ToString()),
                        cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                        pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                        tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                        flr_fecha = Convert.ToDateTime(dr["flr_fecha"].ToString()),
                        flr_estado = Convert.ToInt32(dr["flr_estado"].ToString()),
                        flr_tipoLicencia = dr["flr_tipoLicencia"].ToString(),
                        flr_numeroLicencia = dr["flr_numeroLicencia"].ToString(),
                        flr_categoria = dr["flr_categoria"].ToString(),
                        flr_tipoVehiculo = dr["flr_tipoVehiculo"].ToString(),
                        flr_paisProcede = dr["flr_paisProcede"].ToString(),
                        flr_estadoProcede = dr["flr_estadoProcede"].ToString(),
                        flr_numeroLicencia2 = dr["flr_numeroLicencia2"].ToString(),
                        flr_fechaExpiracion = Convert.ToDateTime(dr["flr_fechaExpiracion"].ToString()),
                        flr_identificacion = dr["flr_identificacion"].ToString(),
                        flr_numeroIdentificacion = dr["flr_numeroIdentificacion"].ToString(),
                        flr_statusLegal = dr["flr_statusLegal"].ToString(),
                        flr_genero = dr["flr_genero"].ToString(),
                        flr_donante = dr["flr_donante"].ToString(),
                        flr_tipoSangre = dr["flr_tipoSangre"].ToString(),
                        flr_talla = dr["flr_talla"].ToString(),
                        flr_peso = dr["flr_peso"].ToString(),
                        flr_tez = dr["flr_tez"].ToString(),
                        flr_colorPelo = dr["flr_colorPelo"].ToString(),
                        flr_colorOjo = dr["flr_colorOjo"].ToString(),
                        flr_nombrePadre = dr["flr_nombrePadre"].ToString(),
                        flr_nombreMadre = dr["flr_nombreMadre"].ToString(),
                        flr_direccion = dr["flr_direccion"].ToString(),
                        flr_numeroDireccion = dr["flr_numeroDireccion"].ToString(),
                        flr_pueblo = dr["flr_pueblo"].ToString(),
                        flr_codigoPostal = dr["flr_codigoPostal"].ToString(),
                        flr_barrio = dr["flr_barrio"].ToString(),
                        flr_apartado = dr["flr_apartado"].ToString(),
                        flr_pueblo2 = dr["flr_pueblo2"].ToString(),
                        flr_codigoPostal2 = dr["flr_codigoPostal2"].ToString(),
                        flr_licenciaSuspendida = dr["flr_licenciaSuspendida"].ToString(),
                        flr_motivoSuspencion = dr["flr_motivoSuspencion"].ToString(),
                        flr_numeroLicenciaPR = dr["flr_numeroLicenciaPR"].ToString(),
                        flr_recluido = dr["flr_recluido"].ToString(),
                        flr_convictoBebida = dr["flr_ConvictoBebida"].ToString(),
                        flr_fechaConvictoBebida = Convert.ToDateTime(dr["flr_fechaConvictoBebida"].ToString()),
                        flr_convictoNarcotico = dr["flr_convictoNarcotico"].ToString(),
                        flr_fechaConvictoNarcotico = Convert.ToDateTime(dr["flr_fechaConvictoNarcotico"].ToString()),
                        flr_obligacionAlimentaria = dr["flr_obligacionAlimentaria"].ToString(),
                        flr_deudaAcca = dr["flr_deudaAcca"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaRec>> AddLicenciaRec(LicenciaRec licenciaRec)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", licenciaRec.cl_cliente.cl_id},
                    { "pg_id", licenciaRec.pg_pago.pg_id},
                    { "tr_id", licenciaRec.tr_tramite.tr_id},
                    { "flr_fecha", licenciaRec.flr_fecha },
                    { "flr_estado", licenciaRec.flr_estado },
                    { "flr_tipoLicencia", licenciaRec.flr_tipoLicencia },
                    { "flr_numeroLicencia", licenciaRec.flr_numeroLicencia },
                    { "flr_categoria", licenciaRec.flr_categoria },
                    { "flr_tipoVehiculo", licenciaRec.flr_tipoVehiculo },
                    { "flr_paisProcede", licenciaRec.flr_paisProcede },
                    { "flr_estadoProcede", licenciaRec.flr_estadoProcede },
                    { "flr_numeroLicencia2", licenciaRec.flr_numeroLicencia2 },
                    { "flr_fechaExpiracion", licenciaRec.flr_fechaExpiracion },
                    { "flr_identificacion", licenciaRec.flr_identificacion },
                    { "flr_numeroIdentificacion", licenciaRec.flr_numeroIdentificacion },
                    { "flr_statusLegal", licenciaRec.flr_statusLegal },
                    { "flr_genero", licenciaRec.flr_genero},
                    { "flr_donante", licenciaRec.flr_donante},
                    { "flr_tipoSangre", licenciaRec.flr_tipoSangre },
                    { "flr_talla", licenciaRec.flr_talla},
                    { "flr_peso", licenciaRec.flr_peso },
                    { "flr_tez", licenciaRec.flr_tez },
                    { "flr_colorPelo", licenciaRec.flr_colorPelo },
                    { "flr_colorOjo", licenciaRec.flr_colorOjo },
                    { "flr_nombrePadre", licenciaRec.flr_nombrePadre },
                    { "flr_nombreMadre", licenciaRec. flr_nombreMadre},
                    { "flr_direccion", licenciaRec.flr_direccion },
                    { "flr_numeroDireccion", licenciaRec.flr_numeroDireccion },
                    { "flr_pueblo", licenciaRec.flr_pueblo },
                    { "flr_codigoPostal", licenciaRec.flr_codigoPostal },
                    { "flr_barrio", licenciaRec.flr_barrio },
                    { "flr_apartado", licenciaRec.flr_apartado},
                    { "flr_pueblo2", licenciaRec.flr_pueblo2},
                    { "flr_codigoPostal2", licenciaRec.flr_codigoPostal2},
                    { "flr_licenciaSuspendida", licenciaRec.flr_licenciaSuspendida},
                    { "flr_motivoSuspencion", licenciaRec.flr_motivoSuspencion},
                    { "flr_numeroLicenciaPR", licenciaRec.flr_numeroLicenciaPR},
                    { "flr_recluido", licenciaRec.flr_recluido},
                    { "flr_convictoBebida", licenciaRec.flr_convictoBebida},
                    { "flr_fechaConvictoBebida", licenciaRec.flr_fechaConvictoBebida == null ? DBNull.Value : licenciaRec.flr_fechaConvictoBebida},
                    { "flr_convictoNarcotico", licenciaRec.flr_convictoNarcotico},
                    { "flr_fechaConvictoNarcotico", licenciaRec.flr_fechaConvictoNarcotico == null ? DBNull.Value : licenciaRec.flr_fechaConvictoNarcotico},
                    { "flr_obligacionAlimentaria", licenciaRec.flr_obligacionAlimentaria},
                    { "flr_deudaAcca", licenciaRec.flr_deudaAcca}
                };
                ResponseEntity<LicenciaRec> result = await Add(licenciaRec, "sp_RegistrarLicenciaRec", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaRec>> UpdateLicenciaRec(LicenciaRec licenciaRec)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "flr_id", licenciaRec.flr_id },
                    { "cl_id", licenciaRec.cl_cliente.cl_id},
                    { "pg_id", licenciaRec.pg_pago.pg_id},
                    { "tr_id", licenciaRec.tr_tramite.tr_id},
                    { "flr_fecha", licenciaRec.flr_fecha },
                    { "flr_estado", licenciaRec.flr_estado },
                    { "flr_tipoLicencia", licenciaRec.flr_tipoLicencia },
                    { "flr_numeroLicencia", licenciaRec.flr_numeroLicencia },
                    { "flr_categoria", licenciaRec.flr_categoria },
                    { "flr_tipoVehiculo", licenciaRec.flr_tipoVehiculo },
                    { "flr_paisProcede", licenciaRec.flr_paisProcede },
                    { "flr_estadoProcede", licenciaRec.flr_estadoProcede },
                    { "flr_numeroLicencia2", licenciaRec.flr_numeroLicencia2 },
                    { "flr_fechaExpiracion", licenciaRec.flr_fechaExpiracion },
                    { "flr_identificacion", licenciaRec.flr_identificacion },
                    { "flr_numeroIdentificacion", licenciaRec.flr_numeroIdentificacion },
                    { "flr_statusLegal", licenciaRec.flr_statusLegal },
                    { "flr_genero", licenciaRec.flr_genero},
                    { "flr_donante", licenciaRec.flr_donante},
                    { "flr_tipoSangre", licenciaRec.flr_tipoSangre },
                    { "flr_talla", licenciaRec.flr_talla},
                    { "flr_peso", licenciaRec.flr_peso },
                    { "flr_tez", licenciaRec.flr_tez },
                    { "flr_colorPelo", licenciaRec.flr_colorPelo },
                    { "flr_colorOjo", licenciaRec.flr_colorOjo },
                    { "flr_nombrePadre", licenciaRec.flr_nombrePadre },
                    { "flr_nombreMadre", licenciaRec. flr_nombreMadre},
                    { "flr_direccion", licenciaRec.flr_direccion },
                    { "flr_numeroDireccion", licenciaRec.flr_numeroDireccion },
                    { "flr_pueblo", licenciaRec.flr_pueblo },
                    { "flr_codigoPostal", licenciaRec.flr_codigoPostal },
                    { "flr_barrio", licenciaRec.flr_barrio },
                    { "flr_apartado", licenciaRec.flr_apartado},
                    { "flr_pueblo2", licenciaRec.flr_pueblo2},
                    { "flr_codigoPostal2", licenciaRec.flr_codigoPostal2},
                    { "flr_licenciaSuspendida", licenciaRec.flr_licenciaSuspendida},
                    { "flr_motivoSuspencion", licenciaRec.flr_motivoSuspencion},
                    { "flr_numeroLicenciaPR", licenciaRec.flr_numeroLicenciaPR},
                    { "flr_recluido", licenciaRec.flr_recluido},
                    { "flr_convictoBebida", licenciaRec.flr_convictoBebida},
                    { "flr_fechaConvictoBebida", licenciaRec.flr_fechaConvictoBebida == null ? DBNull.Value : licenciaRec.flr_fechaConvictoBebida},
                    { "flr_convictoNarcotico", licenciaRec.flr_convictoNarcotico},
                    { "flr_fechaConvictoNarcotico", licenciaRec.flr_fechaConvictoNarcotico == null ? DBNull.Value : licenciaRec.flr_fechaConvictoNarcotico},
                    { "flr_obligacionAlimentaria", licenciaRec.flr_obligacionAlimentaria},
                    { "flr_deudaAcca", licenciaRec.flr_deudaAcca}
                };
                ResponseEntity<LicenciaRec> result = await Update(licenciaRec, "sp_ActualizarLicenciaRec", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<LicenciaRec>> DeleteLicenciaRec(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "flr_id", id }
                };
                ResponseEntity<LicenciaRec> result = await DeleteEntity("sp_EliminaLicenciaRec", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<LicenciaRec> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

    }
}
