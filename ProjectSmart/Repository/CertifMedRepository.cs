using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class CertifMedRepository : BaseRepository<CertifMed>, ICertifMedRepository
    {
        private readonly IConfiguration _configuration;

        public CertifMedRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<CertifMed>> GetAllCertifMed()
        {
            try
            {
                ResponseEntity<CertifMed> result = await GetAllData("sp_ObtenerCertifMed", (SqlDataReader dr) =>
                {
                    return new CertifMed()
                    {                    
                        fcm_id = Convert.ToInt32(dr["fcm_id"].ToString()),
                        cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                        pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                        tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                        frm_id = Convert.ToInt32(dr["frm_id"].ToString()),
                        fcm_numeroSeguro = dr["fcm_numeroSeguro"].ToString(),
                        fcm_numeroLicencia = dr["fcm_numeroLicencia"].ToString(),
                        fcm_ojoDerechoSinLentes = dr["fcm_ojoDerechoSinLentes"].ToString(),
                        fcm_ojoDerechoConLentes = dr["fcm_ojoDerechoConLentes"].ToString(),
                        fcm_ojoIzquierdoSinLentes = dr["fcm_ojoIzquierdoSinLentes"].ToString(),
                        fcm_ojoIzquierdoConLentes = dr["fcm_ojoIzquierdoConLentes"].ToString(),
                        fcm_ambosOjos = dr["fcm_ambosOjos"].ToString(),
                        fcm_condicion = dr["fcm_condicion"].ToString(),
                        fcm_espejuelos = dr["fcm_espejuelos"].ToString(),
                        fcm_usaLentes = dr["fcm_usaLentes"].ToString(),
                        fcm_condicionOido = dr["fcm_condicionOido"].ToString(),
                        fcm_condicionBrazo = dr["fcm_condicionBrazo"].ToString(),
                        fcm_condicionPierna = dr["fcm_condicionPierna"].ToString(),
                        fcm_condicionFisica = dr["fcm_condicionFisica"].ToString(),
                        fcm_observacion = dr["fcm_observacion"].ToString(),
                        fcm_estadoInconciencia = dr["fcm_estadoInconciencia"].ToString(),
                        fcm_padeceCorazon = dr["fcm_padeceCorazon"].ToString(),
                        fcm_marcapaso = dr["fcm_marcapaso"].ToString(),
                        fcm_protesis = dr["fcm_protesis"].ToString(),
                        fcm_estaturaPies = dr["fcm_estaturaPies"].ToString(),
                        fcm_estaturaPulgada = dr["fcm_estaturaPulgada"].ToString(),
                        fcm_peso = dr["fcm_peso"].ToString(),
                        fcm_colorPelo = dr["fcm_colorPelo"].ToString(),
                        fcm_colorOjo = dr["fcm_colorOjo"].ToString(),
                        fcm_nombreMedico = dr["fcm_nombreMedico"].ToString(),
                        fcm_licenciaMedico = dr["fcm_licenciaMedico"].ToString(),
                        fcm_fecha = Convert.ToDateTime(dr["fcm_fecha"].ToString()),
                        fcm_estado = Convert.ToInt32(dr["fcm_estado"].ToString()),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<CertifMed>> GetCertifMedById(int fcm_id)
        {
            try
            {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@fcm_id"] = fcm_id;
            ResponseEntity<CertifMed> result = await GetData("sp_ObtenerCertifMedPorId", parameters, (SqlDataReader dr) =>
            {
                return new CertifMed()
                {
                    fcm_id = Convert.ToInt32(dr["fcm_id"].ToString()), 
                    cl_cliente = new Cliente { cl_id = Convert.IsDBNull(dr["cl_id"]) ? 0 : Convert.ToInt32(dr["cl_id"]) },
                    pg_pago = new Pago { pg_id = Convert.IsDBNull(dr["pg_id"]) ? 0 : Convert.ToInt32(dr["pg_id"]) },
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    frm_id = Convert.ToInt32(dr["frm_id"].ToString()),
                    fcm_numeroSeguro = dr["fcm_numeroSeguro"].ToString(),
                    fcm_numeroLicencia = dr["fcm_numeroLicencia"].ToString(),
                    fcm_ojoDerechoSinLentes = dr["fcm_ojoDerechoSinLentes"].ToString(),
                    fcm_ojoDerechoConLentes = dr["fcm_ojoDerechoConLentes"].ToString(),
                    fcm_ojoIzquierdoSinLentes = dr["fcm_ojoIzquierdoSinLentes"].ToString(),
                    fcm_ojoIzquierdoConLentes = dr["fcm_ojoIzquierdoConLentes"].ToString(),
                    fcm_ambosOjos = dr["fcm_ambosOjos"].ToString(),
                    fcm_condicion = dr["fcm_condicion"].ToString(),
                    fcm_espejuelos = dr["fcm_espejuelos"].ToString(),
                    fcm_usaLentes = dr["fcm_usaLentes"].ToString(),
                    fcm_condicionOido = dr["fcm_condicionOido"].ToString(),
                    fcm_condicionBrazo = dr["fcm_condicionBrazo"].ToString(),
                    fcm_condicionPierna = dr["fcm_condicionPierna"].ToString(),
                    fcm_condicionFisica = dr["fcm_condicionFisica"].ToString(),
                    fcm_observacion = dr["fcm_observacion"].ToString(),
                    fcm_estadoInconciencia = dr["fcm_estadoInconciencia"].ToString(),
                    fcm_padeceCorazon = dr["fcm_padeceCorazon"].ToString(),
                    fcm_marcapaso = dr["fcm_marcapaso"].ToString(),
                    fcm_protesis = dr["fcm_protesis"].ToString(),
                    fcm_estaturaPies = dr["fcm_estaturaPies"].ToString(),
                    fcm_estaturaPulgada = dr["fcm_estaturaPulgada"].ToString(),
                    fcm_peso = dr["fcm_peso"].ToString(),
                    fcm_colorPelo = dr["fcm_colorPelo"].ToString(),
                    fcm_colorOjo = dr["fcm_colorOjo"].ToString(),
                    fcm_nombreMedico = dr["fcm_nombreMedico"].ToString(),
                    fcm_licenciaMedico = dr["fcm_licenciaMedico"].ToString(),
                    fcm_fecha = Convert.ToDateTime(dr["fcm_fecha"].ToString()),
                    fcm_estado = Convert.ToInt32(dr["fcm_estado"].ToString()),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<CertifMed>> AddCertifMed(CertifMed certifMed)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", certifMed.cl_cliente.cl_id},
                    { "pg_id", certifMed.pg_pago.pg_id},
                    { "tr_id", certifMed.tr_tramite.tr_id},
                    { "frm_id", certifMed.frm_id},
                    { "fcm_numeroSeguro", certifMed.fcm_numeroSeguro },
                    { "fcm_numeroLicencia", certifMed.fcm_numeroLicencia },
                    { "fcm_ojoDerechoSinLentes", certifMed.fcm_ojoDerechoSinLentes },
                    { "fcm_ojoDerechoConLentes", certifMed.fcm_ojoDerechoConLentes },
                    { "fcm_ojoIzquierdoSinLentes", certifMed.fcm_ojoIzquierdoSinLentes },
                    { "fcm_ojoIzquierdoConLentes", certifMed.fcm_ojoIzquierdoConLentes },
                    { "fcm_ambosOjos", certifMed.fcm_ambosOjos },
                    { "fcm_condicion", certifMed.fcm_condicion},
                    { "fcm_espejuelos", certifMed.fcm_espejuelos },
                    { "fcm_usaLentes", certifMed.fcm_usaLentes },
                    { "fcm_condicionOido", certifMed.fcm_condicionOido },
                    { "fcm_condicionBrazo", certifMed.fcm_condicionBrazo },
                    { "fcm_condicionPierna", certifMed.fcm_condicionPierna },
                    { "fcm_condicionFisica", certifMed.fcm_condicionFisica },
                    { "fcm_observacion", certifMed.fcm_observacion },
                    { "fcm_estadoInconciencia", certifMed.fcm_estadoInconciencia },
                    { "fcm_padeceCorazon", certifMed.fcm_padeceCorazon },
                    { "fcm_marcapaso", certifMed.fcm_marcapaso },
                    { "fcm_protesis", certifMed.fcm_protesis },
                    { "fcm_estaturaPies", certifMed.fcm_estaturaPies},
                    { "fcm_estaturaPulgada", certifMed.fcm_estaturaPulgada},
                    { "fcm_peso", certifMed.fcm_peso },
                    { "fcm_colorPelo", certifMed.fcm_colorPelo },
                    { "fcm_colorOjo", certifMed.fcm_colorOjo },
                    { "fcm_nombreMedico", certifMed.fcm_nombreMedico },
                    { "fcm_licenciaMedico", certifMed.fcm_licenciaMedico },
                    { "fcm_fecha", certifMed.fcm_fecha },
                    { "fcm_estado", certifMed.fcm_estado },
                };
                ResponseEntity<CertifMed> result = await Add(certifMed, "sp_RegistrarCertifMed", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<CertifMed>> UpdateCertifMed(CertifMed certifMed)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "fcm_id", certifMed.fcm_id},
                    { "cl_id", certifMed.cl_cliente.cl_id},
                    { "pg_id", certifMed.pg_pago.pg_id},
                    { "tr_id", certifMed.tr_tramite.tr_id},
                    { "frm_id", certifMed.frm_id},
                    { "fcm_numeroSeguro", certifMed.fcm_numeroSeguro },
                    { "fcm_numeroLicencia", certifMed.fcm_numeroLicencia },
                    { "fcm_ojoDerechoSinLentes", certifMed.fcm_ojoDerechoSinLentes },
                    { "fcm_ojoDerechoConLentes", certifMed.fcm_ojoDerechoConLentes },
                    { "fcm_ojoIzquierdoSinLentes", certifMed.fcm_ojoIzquierdoSinLentes },
                    { "fcm_ojoIzquierdoConLentes", certifMed.fcm_ojoIzquierdoConLentes },
                    { "fcm_ambosOjos", certifMed.fcm_ambosOjos },
                    { "fcm_condicion", certifMed.fcm_condicion},
                    { "fcm_espejuelos", certifMed.fcm_espejuelos },
                    { "fcm_usaLentes", certifMed.fcm_usaLentes },
                    { "fcm_condicionOido", certifMed.fcm_condicionOido },
                    { "fcm_condicionBrazo", certifMed.fcm_condicionBrazo },
                    { "fcm_condicionPierna", certifMed.fcm_condicionPierna },
                    { "fcm_condicionFisica", certifMed.fcm_condicionFisica },
                    { "fcm_observacion", certifMed.fcm_observacion },
                    { "fcm_estadoInconciencia", certifMed.fcm_estadoInconciencia },
                    { "fcm_padeceCorazon", certifMed.fcm_padeceCorazon },
                    { "fcm_marcapaso", certifMed.fcm_marcapaso },
                    { "fcm_protesis", certifMed.fcm_protesis },
                    { "fcm_estaturaPies", certifMed.fcm_estaturaPies},
                    { "fcm_estaturaPulgada", certifMed.fcm_estaturaPulgada},
                    { "fcm_peso", certifMed.fcm_peso },
                    { "fcm_colorPelo", certifMed.fcm_colorPelo },
                    { "fcm_colorOjo", certifMed.fcm_colorOjo },
                    { "fcm_nombreMedico", certifMed.fcm_nombreMedico },
                    { "fcm_licenciaMedico", certifMed.fcm_licenciaMedico },
                    { "fcm_fecha", certifMed.fcm_fecha },
                    { "fcm_estado", certifMed.fcm_estado },
                };
                ResponseEntity<CertifMed> result = await Update(certifMed, "sp_ActualizarCertifMed", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<CertifMed>> DeleteCertifMed(int id)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "fcm_id", id }
                };
                ResponseEntity<CertifMed> result = await DeleteEntity("sp_EliminaCertifMed", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<CertifMed> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<EstadoCertificado>> CheckCertifiedStatus(int frmId, int trId)
        {
            try
            {
                var parameters = new Dictionary<string, object>();
                parameters.Add("@tr_id", trId);
                parameters.Add("@frm_id", frmId);
                ResponseEntity<EstadoCertificado> response = await GetData("sp_VerificarEstadoCertificadoMedico", parameters, (SqlDataReader dr) =>
                {
                    return new EstadoCertificado
                    { 
                        Estado = dr.IsDBNull(dr.GetOrdinal("estado")) ? string.Empty : dr.GetString(dr.GetOrdinal("estado")),
                        Path = dr.IsDBNull(dr.GetOrdinal("path")) ? string.Empty : dr.GetString(dr.GetOrdinal("path")),
                        Url = dr.IsDBNull(dr.GetOrdinal("url")) ? string.Empty : dr.GetString(dr.GetOrdinal("url")),
                    };
                });
                return response;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<EstadoCertificado> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<EstadoCertificado> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
    }
}
