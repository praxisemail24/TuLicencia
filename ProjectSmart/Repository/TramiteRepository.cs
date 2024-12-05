using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class TramiteRepository : BaseRepository<Tramite>, ITramiteRepository
    {
        public TramiteRepository(IConfiguration configuration) 
            : base(configuration)
        {
        }

        public async Task<ResponseEntity<Tramite>> GetAllTramite()
        {
            try
            {
                ResponseEntity<Tramite> result = await GetAllData("sp_ObtenerTramite", (SqlDataReader dr) =>
                {
                    return new Tramite()
                    {
                        tr_id = Convert.ToInt32(dr["tr_id"].ToString()),
                        tr_nombre = dr["tr_nombre"].ToString(),
                        tr_estado = Convert.ToInt32(dr["tr_estado"].ToString()),
                        tr_precio = Convert.ToDecimal(dr["tr_precio"].ToString()),
                        tr_tipoTramite = new TipoTramite { tpr_id = Convert.IsDBNull(dr["tpr_id"]) ? 0 : Convert.ToInt32(dr["tpr_id"]) },
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Tramite> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Tramite> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public IEnumerable<Tramite> ListarTramites()
        {
            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<Tramite>>("sp_ObtenerTramite", (cmd) =>
            {
                var list = new List<Tramite>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    var t = new Tramite();
                    t.tr_id = reader.IsDBNull(reader.GetOrdinal("tr_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("tr_id"));
                    t.tr_estado = reader.IsDBNull(reader.GetOrdinal("tr_estado")) ? 0 : reader.GetInt32(reader.GetOrdinal("tr_estado"));
                    t.tr_nombre = reader.IsDBNull(reader.GetOrdinal("tr_nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("tr_nombre"));
                    t.tr_precio = reader.IsDBNull(reader.GetOrdinal("tr_precio")) ? 0m : Convert.ToDecimal(reader.GetValue(reader.GetOrdinal("tr_precio")));
                    t.tr_tipoTramite = new TipoTramite
                    {
                        tpr_id = reader.IsDBNull(reader.GetOrdinal("tpr_id")) ? 0 : reader.GetInt32(reader.GetOrdinal("tpr_id"))
                    };
                    list.Add(t);
                }
                return list;
            });
        }
    }

}