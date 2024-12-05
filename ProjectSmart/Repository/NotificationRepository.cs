using SmartLicencia.Data;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class NotificationRepository : AbstractConnection
    {
        public NotificationRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<Notification> UltimosTramites(int adminId)
        {
            try
            {
            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<Notification>>("sp_NotificacionesTramitesAdministrador", cmd =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@admin_id", adminId));
                var reader = cmd.ExecuteReader();
                var lista = new List<Notification>();
                while (reader.Read())
                {
                    var notification = new Notification();
                    notification.Id = reader.IsDBNull(reader.GetOrdinal("id")) ? string.Empty : reader.GetString(reader.GetOrdinal("id"));
                    notification.Title = reader.IsDBNull(reader.GetOrdinal("title")) ? string.Empty : reader.GetString(reader.GetOrdinal("title"));
                    notification.Description = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description"));
                    lista.Add(notification);
                }
                reader.Close();
                return lista;
            });
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error SQL: {sqlEx.Message}");
                return Enumerable.Empty<Notification>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return Enumerable.Empty<Notification>();
            }

        }
    }
}
