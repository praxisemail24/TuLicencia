using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class EstadosRepository : BaseRepository<Pueblos>, IEstadosRepository
    {
        private readonly IConfiguration _configuration;

        public EstadosRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Estados>> GetAllEstados()
        {
            try
            {
                ResponseEntity<Estados> result = await GetAllData("sp_ObtenerEstados", (SqlDataReader dr) =>
                {
                    return new Estados()
                    {
                        e_id = Convert.ToInt32(dr["e_id"].ToString()),
                        e_nombre = dr["e_nombre"].ToString()
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Estados> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Estados> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        //public async Task<ResponseEntity<Pueblos>> GetPueblosById(int pl_id)
        //{
        //    Dictionary<string, object> parameters = new Dictionary<string, object>();
        //    parameters["@pl_id"] = pl_id;
        //    ResponseEntity<Pueblos> result = await GetData("sp_ObtenerPueblosPorId", parameters, (SqlDataReader dr) =>
        //    {
        //        return new Pueblos()
        //        {
        //            pl_id = Convert.ToInt32(dr["pl_id"].ToString()),
        //            pl_nombre = dr["pl_nombre"].ToString(),
        //        };
        //    });
        //    return result;
        //}       
        
    }
}
