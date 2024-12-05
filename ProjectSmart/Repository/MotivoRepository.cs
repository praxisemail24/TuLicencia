using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class MotivoRepository : BaseRepository<Motivo>, IMotivoRepository
    {
        private readonly IConfiguration _configuration;

        public MotivoRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Motivo>> GetAllMotivo()
        {
            try
            {
                ResponseEntity<Motivo> result = await GetAllData("sp_ObtenerMotivo", (SqlDataReader dr) =>
                {
                    return new Motivo()
                    {
                        mot_id = Convert.ToInt32(dr["mot_id"].ToString()),
                        mot_nombre = dr["mot_nombre"].ToString()
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Motivo> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Motivo> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Motivo>> GetMotivoById(int mot_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@mot_id"] = mot_id;
                ResponseEntity<Motivo> result = await GetData("sp_ObtenerMotivoPorId", parameters, (SqlDataReader dr) =>
                {
                    return new Motivo()
                    {
                        mot_id = Convert.ToInt32(dr["mot_id"].ToString()),
                        mot_nombre = dr["mot_nombre"].ToString()
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Motivo> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Motivo> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
    }
}
