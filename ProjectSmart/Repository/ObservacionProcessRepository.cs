using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class ObservacionProcessRepository : BaseRepository<ObservacionProcess>, IObservacionProcessRepository
    {
        private readonly IConfiguration _configuration;

        public ObservacionProcessRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<ObservacionProcess>> AddObservacionProcess(ObservacionProcess observacionProcess)
        {
            try
            {

            var parameters = new Dictionary<string, object>
            {
                { "tr_id", observacionProcess.tr_tramite.tr_id },
                { "frm_id", observacionProcess.frm_form.FrmID },
                { "adm_id", observacionProcess.adm_admin.adm_id },
                { "ob_id", observacionProcess.ob_id },
                { "ob_comentario", observacionProcess.ob_comentario },
                { "ob_estado", observacionProcess.ob_estado },
                { "ob_fecha", observacionProcess.ob_fecha },
                { "mot_id", observacionProcess.mot_id }
            };
            ResponseEntity<ObservacionProcess> result = await Add(observacionProcess, "sp_RegistrarObservacionProcess", parameters);
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<ObservacionProcess> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<ObservacionProcess> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<ObservacionProcess>> GetObservacionProcessById(int ob_id)
        {
            try
            {

            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@ob_id"] = ob_id;
            ResponseEntity<ObservacionProcess> result = await GetData("sp_ObtenerObservacionProcessPorId", parameters, (SqlDataReader dr) =>
            {
                return new ObservacionProcess()
                {
                    tr_tramite = new Tramite { tr_id = Convert.IsDBNull(dr["tr_id"]) ? 0 : Convert.ToInt32(dr["tr_id"]) },
                    frm_form = new FormularioDTO { FrmID = Convert.IsDBNull(dr["frm_id"]) ? 0 : Convert.ToInt32(dr["frm_id"]) },
                    adm_admin = new Administrador { adm_id = Convert.IsDBNull(dr["adm_id"]) ? 0 : Convert.ToInt32(dr["adm_id"]) },

                    ob_id = Convert.ToInt32(dr["ob_id"].ToString()),
                    ob_comentario = dr["ob_comentario"].ToString(),
                    ob_estado = Convert.ToInt32(dr["ob_estado"].ToString()),
                    ob_fecha = Convert.ToDateTime(dr["ob_fecha"].ToString()),
                    mot_id = new Motivo { mot_id = Convert.IsDBNull(dr["mot_id"]) ? 0 : Convert.ToInt32(dr["mot_id"]) },

                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<ObservacionProcess> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<ObservacionProcess> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        
        }
       
    }
}
