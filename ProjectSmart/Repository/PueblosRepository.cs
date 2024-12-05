using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class PueblosRepository : BaseRepository<Pueblos>, IPueblosRepository
    {
        private readonly IConfiguration _configuration;

        public PueblosRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Pueblos>> GetAllPueblos()
        {
            try
            {
                ResponseEntity<Pueblos> result = await GetAllData("sp_ObtenerPueblos", (SqlDataReader dr) =>
                {
                    return new Pueblos()
                    {
                        pl_id = Convert.ToInt32(dr["pl_id"].ToString()),
                        pl_nombre = dr["pl_nombre"].ToString()
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Pueblos> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Pueblos> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Pueblos>> GetPueblosById(int pl_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@pl_id"] = pl_id;
                ResponseEntity<Pueblos> result = await GetData("sp_ObtenerPueblosPorId", parameters, (SqlDataReader dr) =>
                {
                    return new Pueblos()
                    {
                        pl_id = Convert.ToInt32(dr["pl_id"].ToString()),
                        pl_nombre = dr["pl_nombre"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Pueblos> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Pueblos> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }       
        
    }
}
