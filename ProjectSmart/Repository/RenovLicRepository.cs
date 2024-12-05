using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class RenovLicRepository : BaseRepository<RenovLic>, IRenovLicRepository
    {
        private readonly IConfiguration _configuration;

        public RenovLicRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<RenovLic>> GetAllRenovLic()
        {
            try
            {

            ResponseEntity<RenovLic> result = await GetAllData("sp_ObtenerRenovLic", (SqlDataReader dr) =>
            {
                return new RenovLic()
                {
                    frl_id = Convert.ToInt32(dr["frl_id"].ToString()),
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    frl_fecha = Convert.ToDateTime(dr["frl_fecha"].ToString()),
                    frl_estado = Convert.ToInt32(dr["frl_estado"].ToString()),
                    frl_tipoLicencia = dr["frl_tipoLicencia"].ToString(),
                    frl_numeroLicencia = dr["frl_numeroLicencia"].ToString(),
                    frl_categoria = dr["frl_categoria"].ToString(),
                    frl_vehiculoPesado = dr["frl_vehiculoPesado"].ToString(),
                    frl_identificacion = dr["frl_identificacion"].ToString(),
                    frl_numero = dr["frl_numero"].ToString(),
                    frl_StatusLegal = dr["frl_statusLegal"].ToString(),
                    frl_genero = dr["frl_genero"].ToString(),
                    frl_donante = dr["frl_donante"].ToString(),
                    frl_tipoSangre = dr["frl_tipoSangre"].ToString(),
                    frl_talla = dr["frl_talla"].ToString(),
                    frl_peso = dr["frl_peso"].ToString(),
                    frl_tez = dr["frl_tez"].ToString(),
                    frl_colorPelo = dr["frl_colorPelo"].ToString(),
                    frl_colorOjo = dr["frl_colorOjo"].ToString(),
                    frl_direccion = dr["frl_direccion"].ToString(),
                    frl_numeroDireccion = dr["frl_numeroDireccion"].ToString(),
                    frl_pueblo = dr["frl_pueblo"].ToString(),
                    frl_codigoPostal =dr["frl_codigoPostal"].ToString(),
                    frl_barrio = dr["frl_barrio"].ToString(),
                    frl_apartado = dr["frl_apartado"].ToString(),
                    frl_pueblo2 = dr["frl_pueblo2"].ToString(),
                    frl_codigoPostal2 = dr["frl_codigoPostal2"].ToString(),
                    frl_licenciaSuspendida = dr["frl_licenciaSuspendida"].ToString(),
                    frl_motivoSuspension = dr["frl_motivoSuspension"].ToString(),
                    frl_recluido = dr["frl_recluido"].ToString(),
                    frl_convictoBebida = dr["frl_ConvictoBebida"].ToString(),
                    frl_fechaConvictoBebida = !string.IsNullOrEmpty(dr["frl_fechaConvictoBebida"]?.ToString()) ? Convert.ToDateTime(dr["frl_fechaConvictoBebida"]) : DateTime.MinValue,
                    frl_convictoNarcotico = dr["frl_convictoNarcotico"].ToString(),
                    frl_fechaConvictoNarcotico = !string.IsNullOrEmpty(dr["frl_fechaConvictoNarcotico"]?.ToString()) ? Convert.ToDateTime(dr["frl_fechaConvictoNarcotico"]) : DateTime.MinValue,
                    frl_obligacionAlimentaria = dr["frl_obligacionAlimentaria"].ToString(),
                    frl_deudaAcca = dr["frl_deudaAcca"].ToString(),
                    frl_paisProcede = dr.IsDBNull(dr.GetOrdinal("frl_paisProcede")) ? "" : dr.GetString(dr.GetOrdinal("frl_paisProcede")),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<RenovLic>> GetRenovLicById(int frl_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@frl_id"] = frl_id;
                ResponseEntity<RenovLic> result = await GetData("sp_ObtenerRenovLicPorId", parameters, (SqlDataReader dr) =>
                {
                    return new RenovLic()
                    {
                        frl_id = Convert.ToInt32(dr["frl_id"].ToString()),
                        cl_cliente = new Cliente {cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"])},
                        pg_pago = new Pago {pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                        tr_tramite = new Tramite {tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"])},
                        frl_fecha = Convert.ToDateTime(dr["frl_fecha"].ToString()),
                        frl_estado = Convert.ToInt32(dr["frl_estado"].ToString()),
                        frl_tipoLicencia = dr["frl_tipoLicencia"].ToString(),
                        frl_numeroLicencia = dr["frl_numeroLicencia"].ToString(),
                        frl_categoria = dr["frl_categoria"].ToString(),
                        frl_vehiculoPesado = dr["frl_vehiculoPesado"].ToString(),
                        frl_identificacion = dr["frl_identificacion"].ToString(),
                        frl_numero = dr["frl_numero"].ToString(),
                        frl_StatusLegal = dr["frl_statusLegal"].ToString(),
                        frl_genero = dr["frl_genero"].ToString(),
                        frl_donante = dr["frl_donante"].ToString(),
                        frl_tipoSangre = dr["frl_tipoSangre"].ToString(),
                        frl_talla = dr["frl_talla"].ToString(),
                        frl_peso = dr["frl_peso"].ToString(),
                        frl_tez = dr["frl_tez"].ToString(),
                        frl_colorPelo = dr["frl_colorPelo"].ToString(),
                        frl_colorOjo = dr["frl_colorOjo"].ToString(),
                        frl_direccion = dr["frl_direccion"].ToString(),
                        frl_numeroDireccion = dr["frl_numeroDireccion"].ToString(),
                        frl_pueblo = dr["frl_pueblo"].ToString(),
                        frl_codigoPostal = dr["frl_codigoPostal"].ToString(),
                        frl_barrio = dr["frl_barrio"].ToString(),
                        frl_apartado = dr["frl_apartado"].ToString(),
                        frl_pueblo2 = dr["frl_pueblo2"].ToString(),
                        frl_codigoPostal2 = dr["frl_codigoPostal2"].ToString(),
                        frl_licenciaSuspendida = dr["frl_licenciaSuspendida"].ToString(),
                        frl_motivoSuspension = dr["frl_motivoSuspension"].ToString(),
                        frl_recluido = dr["frl_recluido"].ToString(),
                        frl_convictoBebida = dr["frl_ConvictoBebida"].ToString(),
                        frl_fechaConvictoBebida = !string.IsNullOrEmpty(dr["frl_fechaConvictoBebida"]?.ToString()) ? Convert.ToDateTime(dr["frl_fechaConvictoBebida"]) : DateTime.MinValue,
                        frl_convictoNarcotico = dr["frl_convictoNarcotico"].ToString(),
                        frl_fechaConvictoNarcotico = !string.IsNullOrEmpty(dr["frl_fechaConvictoNarcotico"]?.ToString()) ? Convert.ToDateTime(dr["frl_fechaConvictoNarcotico"]) : DateTime.MinValue,
                        frl_obligacionAlimentaria = dr["frl_obligacionAlimentaria"].ToString(),
                        frl_deudaAcca = dr["frl_deudaAcca"].ToString(),
                        frl_paisProcede = dr.IsDBNull(dr.GetOrdinal("frl_paisProcede")) ? "" : dr.GetString(dr.GetOrdinal("frl_paisProcede")),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<RenovLic>> AddRenovLic(RenovLic renovLic)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", renovLic.cl_cliente.cl_id},
                    { "pg_id", renovLic.pg_pago.pg_id},
                    { "tr_id", renovLic.tr_tramite.tr_id},
                    { "frl_fecha", renovLic.frl_fecha },
                    { "frl_estado", renovLic.frl_estado },
                    { "frl_tipoLicencia", renovLic.frl_tipoLicencia },
                    { "frl_numeroLicencia", renovLic.frl_numeroLicencia },
                    { "frl_categoria", renovLic.frl_categoria },
                    { "frl_vehiculoPesado", renovLic.frl_vehiculoPesado },
                    { "frl_identificacion", renovLic.frl_identificacion },
                    { "frl_numero", renovLic.frl_numero },
                    { "frl_statusLegal", renovLic.frl_StatusLegal },
                    { "frl_genero", renovLic.frl_genero},
                    { "frl_donante", renovLic.frl_donante},
                    { "frl_tipoSangre", renovLic.frl_tipoSangre },
                    { "frl_talla", renovLic.frl_talla},
                    { "frl_peso", renovLic.frl_peso },
                    { "frl_tez", renovLic.frl_tez },
                    { "frl_colorPelo", renovLic.frl_colorPelo },
                    { "frl_colorOjo", renovLic.frl_colorOjo },
                    { "frl_direccion", renovLic.frl_direccion },
                    { "frl_numeroDireccion", renovLic.frl_numeroDireccion },
                    { "frl_pueblo", renovLic.frl_pueblo },
                    { "frl_codigoPostal", renovLic.frl_codigoPostal },
                    { "frl_barrio", renovLic.frl_barrio },
                    { "frl_apartado", renovLic.frl_apartado},
                    { "frl_pueblo2", renovLic.frl_pueblo2},
                    { "frl_codigoPostal2", renovLic.frl_codigoPostal2},
                    { "frl_licenciaSuspendida", renovLic.frl_licenciaSuspendida},
                    { "frl_motivoSuspension", renovLic.frl_motivoSuspension}, 
                    { "frl_recluido", renovLic.frl_recluido},
                    { "frl_convictoBebida", renovLic.frl_convictoBebida},
                    { "frl_fechaConvictoBebida", renovLic.frl_fechaConvictoBebida == null ? DBNull.Value : renovLic.frl_fechaConvictoBebida },
                    { "frl_convictoNarcotico", renovLic.frl_convictoNarcotico},
                    { "frl_fechaConvictoNarcotico", renovLic.frl_fechaConvictoNarcotico == null ? DBNull.Value : renovLic.frl_fechaConvictoNarcotico },
                    { "frl_obligacionAlimentaria", renovLic.frl_obligacionAlimentaria},
                    { "frl_deudaAcca", renovLic.frl_deudaAcca},
                    { "frl_estadoProceso", renovLic.frl_estadoProceso },
                    { "frl_estadoRevision", renovLic.frl_estadoRevision },
                    { "frl_paisProcede", renovLic.frl_paisProcede == null ? DBNull.Value : renovLic.frl_paisProcede}
                };
                ResponseEntity<RenovLic> result = await Add(renovLic, "sp_RegistrarRenovLic", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<RenovLic>> UpdateRenovLic(RenovLic renovLic)
        {
            try
            {

            var parameters = new Dictionary<string, object>
            {
                { "frl_id", renovLic.frl_id },
                { "cl_id", renovLic.cl_cliente.cl_id},
                { "pg_id", renovLic.pg_pago.pg_id},
                { "tr_id", renovLic.tr_tramite.tr_id},
                { "frl_fecha", renovLic.frl_fecha },
                { "frl_estado", renovLic.frl_estado },
                { "frl_tipoLicencia", renovLic.frl_tipoLicencia },
                { "frl_numeroLicencia", renovLic.frl_numeroLicencia },
                { "frl_categoria", renovLic.frl_categoria },
                { "frl_vehiculoPesado", renovLic.frl_vehiculoPesado },
                { "frl_identificacion", renovLic.frl_identificacion },
                { "frl_numero", renovLic.frl_numero },
                { "frl_statusLegal", renovLic.frl_StatusLegal },
                { "frl_genero", renovLic.frl_genero},
                { "frl_donante", renovLic.frl_donante},
                { "frl_tipoSangre", renovLic.frl_tipoSangre },
                { "frl_talla", renovLic.frl_talla},
                { "frl_peso", renovLic.frl_peso },
                { "frl_tez", renovLic.frl_tez },
                { "frl_colorPelo", renovLic.frl_colorPelo },
                { "frl_colorOjo", renovLic.frl_colorOjo },
                { "frl_direccion", renovLic.frl_direccion },
                { "frl_numeroDireccion", renovLic.frl_numeroDireccion },
                { "frl_pueblo", renovLic.frl_pueblo },
                { "frl_codigoPostal", renovLic.frl_codigoPostal },
                { "frl_barrio", renovLic.frl_barrio },
                { "frl_apartado", renovLic.frl_apartado},
                { "frl_pueblo2", renovLic.frl_pueblo2},
                { "frl_codigoPostal2", renovLic.frl_codigoPostal2},
                { "frl_licenciaSuspendida", renovLic.frl_licenciaSuspendida},
                { "frl_motivoSuspension", renovLic.frl_motivoSuspension},
                { "frl_recluido", renovLic.frl_recluido},
                { "frl_convictoBebida", renovLic.frl_convictoBebida},
                { "frl_fechaConvictoBebida", renovLic.frl_fechaConvictoBebida == null ? DBNull.Value : renovLic.frl_fechaConvictoBebida},
                { "frl_convictoNarcotico", renovLic.frl_convictoNarcotico},
                { "frl_fechaConvictoNarcotico", renovLic.frl_fechaConvictoNarcotico == null ? DBNull.Value : renovLic.frl_fechaConvictoNarcotico},
                { "frl_obligacionAlimentaria", renovLic.frl_obligacionAlimentaria},
                { "frl_deudaAcca", renovLic.frl_deudaAcca},
                { "frl_paisProcede", renovLic.frl_paisProcede == null ? DBNull.Value : renovLic.frl_paisProcede}

            };
            ResponseEntity<RenovLic> result = await Update(renovLic, "sp_ActualizarRenovLic", parameters);
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<RenovLic>> DeleteRenovLic(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "frl_id", id }
                };
                ResponseEntity<RenovLic> result = await DeleteEntity("sp_EliminaRenovLic", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
        
        public async Task<ResponseEntity<RenovLic>> GetRenovLicValidacion(int cl_id, int pg_id, int tr_id)
        {
            try
            {

                string connectionString = _configuration.GetConnectionString("Conexion");
                using (var connection = new SqlConnection(connectionString))
                {
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters["@cl_id"] = cl_id;
                    parameters["@pg_id"] = pg_id;
                    parameters["@tr_id"] = tr_id;

                    string sqlQuery = "SELECT COUNT(*) FROM Frm_RenovacionLicencia WHERE cl_id = @cl_id AND pg_id = @pg_id AND tr_id = @tr_id";
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        foreach (var parameter in parameters)
                        {
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        }
                        int count = Convert.ToInt32(await command.ExecuteScalarAsync());
                        if (count > 0)
                        {
                            return new ResponseEntity<RenovLic> { success = true, message = "Existe registro" };
                        }
                        else
                        {
                            return new ResponseEntity<RenovLic> { success = false, message = "No hay registro" };
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<RenovLic> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<FormularioDTO>> GetFormEstado0()
        {
            try
            {

                ResponseEntity<FormularioDTO> result = await GetAllData("spPanel_ObtenerFormEstad0", (SqlDataReader dr) =>
                {
                    return new FormularioDTO()
                    {
                        FrmID = Convert.ToInt32(dr["id"].ToString()),
                        cl_cliente = new Cliente
                        {
                            cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                            cl_nombre = dr["nombreCliente"].ToString(),
                            cl_correo = dr["correo"].ToString(),
                            cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                        },
                        cl_pago = new Pago
                        {
                            pg_codigo = dr["codigoPago"].ToString()
                        },
                        Fecha = Convert.ToDateTime(dr["fecha"].ToString()),
                        Estado = Convert.ToInt32(dr["estado"].ToString()),
                        TipoTramite = dr["tipoTramite"].ToString(),
                        NombreTramite = dr["nombreTramite"].ToString(),
                        cl_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<FormularioDTO>> GetFormEstado1()
        {
            try
            {

                ResponseEntity<FormularioDTO> result = await GetAllData("spPanel_ObtenerFormEstad1", (SqlDataReader dr) =>
                {
                    return new FormularioDTO()
                    {
                        FrmID = Convert.ToInt32(dr["id"].ToString()),
                        cl_cliente = new Cliente
                        {
                            cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                            cl_nombre = dr["nombreCliente"].ToString(),
                            cl_correo = dr["correo"].ToString(),
                            cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                        },
                        cl_pago = new Pago
                        {
                            pg_codigo = dr["codigoPago"].ToString()
                        },
                        Fecha = Convert.ToDateTime(dr["fecha"].ToString()),
                        Estado = Convert.ToInt32(dr["estado"].ToString()),
                        TipoTramite = dr["tipoTramite"].ToString(),
                        NombreTramite = dr["nombreTramite"].ToString(),
                        cl_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    };
                });
                return result;

            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<FormularioDTO>> GetFormEstado2()
        {
            try
            {

                ResponseEntity<FormularioDTO> result = await GetAllData("spPanel_ObtenerFormEstad2", (SqlDataReader dr) =>
                {
                    return new FormularioDTO()
                    {
                        FrmID = Convert.ToInt32(dr["id"].ToString()),
                        cl_cliente = new Cliente
                        {
                            cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                            cl_nombre = dr["nombreCliente"].ToString(),
                            cl_correo = dr["correo"].ToString(),
                            cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                        },
                        cl_pago = new Pago
                        {
                            pg_codigo = dr["codigoPago"].ToString()
                        },
                        Fecha = Convert.ToDateTime(dr["fecha"].ToString()),
                        Estado = Convert.ToInt32(dr["estado"].ToString()),
                        TipoTramite = dr["tipoTramite"].ToString(),
                        NombreTramite = dr["nombreTramite"].ToString(),
                        cl_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<FormularioDTO>> GetFormEstado3()
        {
            try
            {

                ResponseEntity<FormularioDTO> result = await GetAllData("spPanel_ObtenerFormEstad3", (SqlDataReader dr) =>
                {
                    return new FormularioDTO()
                    {
                        FrmID = Convert.ToInt32(dr["id"].ToString()),
                        cl_cliente = new Cliente
                        {
                            cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                            cl_nombre = dr["nombreCliente"].ToString(),
                            cl_correo = dr["correo"].ToString(),
                            cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                        },
                        cl_pago = new Pago
                        {
                            pg_codigo = dr["codigoPago"].ToString()
                        },
                        Fecha = Convert.ToDateTime(dr["fecha"].ToString()),
                        Estado = Convert.ToInt32(dr["estado"].ToString()),
                        TipoTramite = dr["tipoTramite"].ToString(),
                        NombreTramite = dr["nombreTramite"].ToString(),
                        cl_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<FormularioDTO>> GetDatosCompletoForm(int cl_id, int tr_id, int frm_id)
        {
            try
            {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@cl_id"] = cl_id;
            parameters["@tr_id"] = tr_id;
            parameters["@frm_id"] = frm_id;
            ResponseEntity<FormularioDTO> result = await GetData("spPanel_ObtenerDatosCompletosCliente", parameters, (SqlDataReader dr) =>
            {
                return new FormularioDTO()
                {
                    FrmID = Convert.ToInt32(dr["frm_id"].ToString()),
                    cl_cliente = new Cliente {
                        cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                        cl_numeroSeguro = dr.IsDBNull(dr.GetOrdinal("cl_numeroSeguro")) ? string.Empty : dr.GetString("cl_numeroSeguro"),
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_correo = dr["cl_correo"].ToString(),
                        cl_fechaNacimiento = Convert.ToDateTime(dr["cl_fechaNacimiento"].ToString()),
                        cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                    },
                    cl_pueblos = new Pueblos
                    {
                        pl_id = Convert.IsDBNull(dr["pl_id"]) ? 0 : Convert.ToInt32(dr["pl_id"]),
                        pl_nombre = dr["pl_nombre"].ToString()
                    },
                    cl_pago = new Pago
                    {
                        pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]),
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_fecha = Convert.ToDateTime(dr["pg_fecha"].ToString()),
                    },                    
                    Fecha = Convert.ToDateTime(dr["frm_fecha"].ToString()),
                    Estado = Convert.ToInt32(dr["frm_estado"]),
                    TipoLicencia = dr["frm_tipoLicencia"].ToString(),
                    NumeroLicencia = dr["frm_numeroLicencia"].ToString(),
                    Categoria = dr["frm_categoria"].ToString(),
                    VehiculoPesado = dr["frm_vehiculoPesado"].ToString(),
                    Identificacion = dr["frm_identificacion"].ToString(),
                    Numero = dr["frm_numero"].ToString(),
                    StatusLegal = dr["frm_statusLegal"].ToString(),
                    Genero = dr["frm_genero"].ToString(),
                    Donante = dr["frm_donante"].ToString(),
                    TipoSangre = dr["frm_tipoSangre"].ToString(),
                    Talla = dr["frm_talla"].ToString(),
                    Peso = dr["frm_peso"].ToString(),
                    Tez = dr["frm_tez"].ToString(),
                    ColorPelo = dr["frm_colorPelo"].ToString(),
                    ColorOjo = dr["frm_colorOjo"].ToString(),
                    Direccion = dr["frm_direccion"].ToString(),
                    NumeroDireccion = dr["frm_numeroDireccion"].ToString(),
                    Pueblo = dr["frm_pueblo"].ToString(),
                    CodigoPostal = dr["frm_codigoPostal"].ToString(),
                    Barrio = dr["frm_barrio"].ToString(),
                    Apartado = dr["frm_apartado"].ToString(),
                    Pueblo2 = dr["frm_pueblo2"].ToString(),
                    CodigoPostal2 = dr["frm_codigoPostal2"].ToString(),
                    LicenciaSuspendida = dr["frm_licenciaSuspendida"].ToString(),
                    MotivoSuspension = dr["frm_motivoSuspension"].ToString(),
                    Recluido = dr["frm_recluido"].ToString(),
                    ConvictoBebida = dr["frm_ConvictoBebida"].ToString(),
                    FechaConvictoBebida = !string.IsNullOrEmpty(dr["frm_fechaConvictoBebida"]?.ToString()) ? Convert.ToDateTime(dr["frm_fechaConvictoBebida"]) : null,
                    ConvictoNarcotico = dr["frm_convictoNarcotico"].ToString(),
                    FechaConvictoNarcotico = !string.IsNullOrEmpty(dr["frm_fechaConvictoNarcotico"]?.ToString()) ? Convert.ToDateTime(dr["frm_fechaConvictoNarcotico"]) : null,
                    ObligacionAlimentaria = dr["frm_obligacionAlimentaria"].ToString(),
                    DeudaAcca = dr["frm_deudaAcca"].ToString(),
                    
                    TipoVehiculo = dr["frm_tipoVehiculo"].ToString(),
                    PaisProcede = dr["frm_paisProcede"].ToString(),
                    EstadoProcede = dr["frm_estadoProcede"].ToString(),
                    NumeroLicencia2 = dr["frm_numeroLicencia2"].ToString(),
                    FechaExpiracion = !string.IsNullOrEmpty(dr["frm_fechaExpiracion"]?.ToString()) ? Convert.ToDateTime(dr["frm_fechaExpiracion"]) : null,
                    NumeroIdentificacion = dr["frm_numeroIdentificacion"].ToString(),
                    NombrePadre = dr["frm_nombrePadre"].ToString(),
                    NombreMadre = dr["frm_nombreMadre"].ToString(),
                    NumeroLicenciaPR = dr["frm_numeroLicenciaPR"].ToString(),
                    EstadoProceso = dr.IsDBNull(dr.GetOrdinal("frm_estadoProceso")) ? 0 : dr.GetInt32(dr.GetOrdinal("frm_estadoProceso")),
                    EstadoRevision = dr.IsDBNull(dr.GetOrdinal("frm_estadoRevision")) ? 0 : dr.GetInt32(dr.GetOrdinal("frm_estadoRevision")),
                    doctorAsignado = dr["doctorAsignado"].ToString(),
                    estadoFormulario = dr["estadoFormulario"].ToString(),
                    estadoMultas = dr["estadoMultas"].ToString(),
                    estadoEvaluacion = dr["estadoEvaluacion"].ToString()
                    

                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<FormularioDTO>> UpdateDatosCompletoFormPanel(FormularioDTO formularioDTO)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "frm_id", formularioDTO.FrmID },
                    { "tr_id", formularioDTO.tr_id },
                    { "frm_estado", formularioDTO.Estado },
                    //{ "frm_fecha", formularioDTO.Fecha == null ? DBNull.Value : formularioDTO.Fecha},
                    { "frm_tipoLicencia", formularioDTO.TipoLicencia },
                    { "frm_numeroLicencia", formularioDTO.NumeroLicencia },
                    { "frm_categoria", formularioDTO.Categoria },
                    { "frm_vehiculoPesado", formularioDTO.VehiculoPesado },
                    { "frm_identificacion", formularioDTO.Identificacion },
                    { "frm_numero", formularioDTO.Numero },
                    { "frm_statusLegal", formularioDTO.StatusLegal },
                    { "frm_genero", formularioDTO.Genero },
                    { "frm_donante", formularioDTO.Donante },
                    { "frm_tipoSangre", formularioDTO.TipoSangre },
                    { "frm_talla", formularioDTO.Talla },
                    { "frm_peso", formularioDTO.Peso },
                    { "frm_tez", formularioDTO.Tez },
                    { "frm_colorPelo", formularioDTO.ColorPelo },
                    { "frm_colorOjo", formularioDTO.ColorOjo },
                    { "frm_direccion", formularioDTO.Direccion },
                    { "frm_numeroDireccion", formularioDTO.NumeroDireccion },
                    { "frm_pueblo", formularioDTO.Pueblo },
                    { "frm_barrio", formularioDTO.Barrio },
                    { "frm_apartado", formularioDTO.Apartado },
                    { "frm_pueblo2", formularioDTO.Pueblo2 },
                    { "frm_licenciaSuspendida", formularioDTO.LicenciaSuspendida },
                    { "frm_motivoSuspension", formularioDTO.MotivoSuspension },
                    { "frm_recluido", formularioDTO.Recluido },
                    { "frm_convictoBebida", formularioDTO.ConvictoBebida },
                    { "frm_fechaConvictoBebida", formularioDTO.FechaConvictoBebida == null ? DBNull.Value : formularioDTO.FechaConvictoBebida },
                    { "frm_convictoNarcotico", formularioDTO.ConvictoNarcotico },
                    { "frm_fechaConvictoNarcotico", formularioDTO.FechaConvictoNarcotico == null ? DBNull.Value : formularioDTO.FechaConvictoNarcotico },
                    { "frm_obligacionAlimentaria", formularioDTO.ObligacionAlimentaria },
                    { "frm_deudaAcca", formularioDTO.DeudaAcca },
                    { "frm_codigoPostal", formularioDTO.CodigoPostal },
                    { "frm_codigoPostal2", formularioDTO.CodigoPostal2 },

                    { "frm_tipoVehiculo", formularioDTO.TipoVehiculo },
                    { "frm_paisProcede", formularioDTO.PaisProcede },
                    { "frm_estadoProcede", formularioDTO.EstadoProcede },
                    { "frm_numeroLicencia2", formularioDTO.NumeroLicencia2 },
                    { "frm_fechaExpiracion", formularioDTO.FechaExpiracion == null ? DBNull.Value : formularioDTO.FechaExpiracion },
                    { "frm_numeroIdentificacion", formularioDTO.NumeroIdentificacion },
                    { "frm_nombrePadre", formularioDTO.NombrePadre },
                    { "frm_nombreMadre", formularioDTO.NombreMadre },
                    { "frm_numeroLicenciaPR", formularioDTO.NumeroLicenciaPR },
                    { "frm_estadoRevision", formularioDTO.EstadoRevision },
                    { "frm_estadoProceso", formularioDTO.EstadoProceso },
                    { "estadoMultas", formularioDTO.estadoMultas },

                };
                ResponseEntity<FormularioDTO> result = await Update(formularioDTO, "spPanel_ActualizarFormularioCliente", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<int>> cambioEstadoForm(int tr_id, int frm_id, int frm_estado )
        {
            try
            {

                var parameters = new Dictionary<string, object>
                {
                    { "@frm_id", frm_id },
                    { "@tr_id", tr_id },
                    { "@new_estado", frm_estado },
              
                };
                ResponseEntity<int> result = await Update<int>(frm_estado, "spPanel_cambioEstadoForm", parameters);
                return result;

            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<int> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<int> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        //public async Task<ResponseEntity<int>> cambioEstadoProcesoForm(int tr_id, int frm_id, int frm_estadoPRoceso)
        //{
        //    var parameters = new Dictionary<string, object>
        //    {
        //        { "@frm_id", frm_id },
        //        { "@tr_id", tr_id },
        //        { "@new_estado", frm_estadoPRoceso },
        //    };
        //    ResponseEntity<int> result = await Update<int>(frm_estadoPRoceso, "spPanel_cambioEstadoProcesoForm", parameters);
        //    return result;
        //}
        public async Task<ResponseEntity<int>> cambioEstadoProcesoForm(int tr_id, int frm_id, int frm_estadoProceso, string motivo)
        {
            try
            {

            var parameters = new Dictionary<string, object>
            {
                { "@frm_id", frm_id },
                { "@tr_id", tr_id },
                { "@new_estado", frm_estadoProceso },
                { "@motivo", motivo }
            };
            ResponseEntity<int> result = await Update<int>(frm_estadoProceso, "spPanel_cambioEstadoProcesoForm", parameters);
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<int> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<int> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<FormularioDTO>> GetObtenerRegistro1()
        {
            try
            {

            ResponseEntity<FormularioDTO> result = await GetAllData("sp_PanelObtenerRegistro1", (SqlDataReader dr) =>
            {
                return new FormularioDTO()
                {
                    FrmID = int.TryParse(dr["frm_id"].ToString(), out int frmId) ? frmId : 0,

                    cl_cliente = new Cliente
                    {
                        cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]),
                        cl_nombreCompleto = dr["nombreCliente"].ToString(),
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_correo = dr["cl_correo"].ToString(),
                        cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                    },
                    cl_pago = new Pago
                    {
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_fecha = !string.IsNullOrEmpty(dr["pg_fecha"]?.ToString()) ? Convert.ToDateTime(dr["pg_fecha"]) : DateTime.MinValue

                    },
                    Estado = int.TryParse(dr["frm_estado"].ToString(), out int estado) ? estado : 0,
                    EstadoProceso = int.TryParse(dr["frm_estadoProceso"].ToString(), out int estadoProceso) ? estadoProceso : 0,
                    NombreTramite = dr["nombreTramite"].ToString(),
                    cl_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<FormularioDTO>> GetBuscarReportePanel(FormularioDTO item, PaginatorEntity paginator)
        {
            try
            {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@cl_nombre"] = item.cl_cliente.cl_nombre;
            parameters["@cl_primerApellido"] = item.cl_cliente.cl_primerApellido;
            parameters["@cl_segundoApellido"] = item.cl_cliente.cl_segundoApellido;
            parameters["@cl_correo"] = item.cl_cliente.cl_correo;
            parameters["@cl_numeroTelefono"] = item.cl_cliente.cl_numeroTelefono;
            parameters["@tr_id"] = item.cl_tramite.tr_id;
            parameters["@pg_fecha"] = null;
            parameters["@pg_codigo"] = item.cl_pago.pg_codigo;
            //parameters["@pg_fecha"] = item.cl_pago.pg_fecha;
            parameters["@frm_estado"] = item.Estado;
            parameters["@frm_estadoProceso"] = item.EstadoProceso;
            parameters["@pstart"] = paginator.offset;
            parameters["@plimit"] = paginator.limit;
            //parameters["@pordercolumn"] = paginator.Sort;
            //parameters["@porderdir"] = paginator.Order;

            ResponseEntity<FormularioDTO> result = await GetAllDataPaginator("spPanel_BuscarRegistro1", (SqlDataReader dr) =>
            {
                return new FormularioDTO()
                {
                    FrmID = dr["frm_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["frm_id"]),
                    cl_cliente = new Cliente
                    {
                        cl_id = dr["cl_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["cl_id"]),
                        cl_nombreCompleto = dr["nombreCliente"].ToString(),
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_correo = dr["cl_correo"].ToString(),
                        cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                    },
                    cl_pago = new Pago
                    {
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_fecha = !string.IsNullOrEmpty(dr["pg_fecha"]?.ToString()) ? Convert.ToDateTime(dr["pg_fecha"]) : DateTime.MinValue
                    },
                    Estado = dr["frm_estado"] == DBNull.Value ? 0 : Convert.ToInt32(dr["frm_estado"]),
                    EstadoProceso = dr["frm_estadoProceso"] == DBNull.Value ? 0 : Convert.ToInt32(dr["frm_estadoProceso"]),
                    NombreTramite = dr["nombreTramite"] != DBNull.Value ? dr["nombreTramite"].ToString() : "",
                    cl_tramite = new Tramite { tr_id = dr["tr_id"] != DBNull.Value ? Convert.ToInt32(dr["tr_id"]) : 0 },
                    PorcAvance = dr.IsDBNull(dr.GetOrdinal("porcAvance")) ? 0 : dr.GetDecimal(dr.GetOrdinal("porcAvance"))
                };
            }, parameters);
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<FormularioDTO>> GetBuscarReportePanelExcel(FormularioDTO item)
        {
            try
            {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@cl_nombre"] = item.cl_cliente.cl_nombre;
            parameters["@cl_primerApellido"] = item.cl_cliente.cl_primerApellido;
            parameters["@cl_segundoApellido"] = item.cl_cliente.cl_segundoApellido;
            parameters["@cl_correo"] = item.cl_cliente.cl_correo;
            parameters["@cl_numeroTelefono"] = item.cl_cliente.cl_numeroTelefono;
            parameters["@tr_id"] = item.cl_tramite.tr_id;
            parameters["@pg_fecha"] = null;
            parameters["@pg_codigo"] = item.cl_pago.pg_codigo;
            parameters["@frm_estado"] = item.Estado;
            parameters["@frm_estadoProceso"] = item.EstadoProceso;

            ResponseEntity<FormularioDTO> result = await GetAllDataById("spPanel_BuscarRegistroExcel1", (SqlDataReader dr) =>
            {
                return new FormularioDTO()
                {
                    FrmID = dr["frm_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["frm_id"]),
                    cl_cliente = new Cliente
                    {
                        cl_id = dr["cl_id"] == DBNull.Value ? 0 : Convert.ToInt32(dr["cl_id"]),
                        cl_nombreCompleto = dr["nombreCliente"].ToString(),
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_correo = dr["cl_correo"].ToString(),
                        cl_numeroTelefono = dr["cl_numeroTelefono"].ToString()
                    },
                    cl_pago = new Pago
                    {
                        pg_codigo = dr["pg_codigo"].ToString(),
                        pg_fecha = !string.IsNullOrEmpty(dr["pg_fecha"]?.ToString()) ? Convert.ToDateTime(dr["pg_fecha"]) : DateTime.MinValue
                    },
                    Estado = dr["frm_estado"] == DBNull.Value ? 0 : Convert.ToInt32(dr["frm_estado"]),
                    EstadoProceso = dr["frm_estadoProceso"] == DBNull.Value ? 0 : Convert.ToInt32(dr["frm_estadoProceso"]),
                    NombreTramite = dr["nombreTramite"] != DBNull.Value ? dr["nombreTramite"].ToString() : "",
                    cl_tramite = new Tramite { tr_id = dr["tr_id"] != DBNull.Value ? Convert.ToInt32(dr["tr_id"]) : 0 }
                };
            }, parameters);
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<FormularioDTO> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

    }

}
