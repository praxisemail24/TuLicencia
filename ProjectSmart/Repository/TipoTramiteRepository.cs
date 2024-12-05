using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class TipoTramiteRepository : BaseRepository<TipoTramite>, ITipoTramiteRepository
    {
        private readonly IConfiguration _configuration;

        public TipoTramiteRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<TipoTramite>> GetAllTipoTramite()
        {
            try
            {
                ResponseEntity<TipoTramite> result = await GetAllData("sp_ObtenerTipoTramite", (SqlDataReader dr) =>
                {
                    return new TipoTramite()
                    {
                        tpr_id = Convert.ToInt32(dr["tpr_id"].ToString()),
                        tpr_nombre = dr["tpr_nombre"].ToString(),
                        tpr_estado = dr["tpr_estado"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<TipoTramite> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<TipoTramite> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
    }
}
