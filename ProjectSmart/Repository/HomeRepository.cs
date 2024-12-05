using SmartLicencia.Data;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class HomeRepository : AbstractConnection
    {
        public HomeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<ReporteTramiteCaso> TramitesInicio(int adminId, int status, string? pago, int? tipoTramite, string? nombreTramite, string? EstadoProceso)
        {
            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<ReporteTramiteCaso>>("sp_ListarTramitesInicio", cmd =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@admin_id", adminId));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@estado", status));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@codigo_pago", pago));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@tipo_tramite", tipoTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@nombre_tramite", nombreTramite));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@EstadoProceso", EstadoProceso));
                var reader = cmd.ExecuteReader();
                List<ReporteTramiteCaso> lista = new List<ReporteTramiteCaso>();
                while (reader.Read())
                {
                    var item = new ReporteTramiteCaso();
                    item.Id = reader.GetInt64(reader.GetOrdinal("id"));
                    item.ClienteId = reader.GetInt32(reader.GetOrdinal("cl_id"));
                    item.PagoCodigo = reader.IsDBNull(reader.GetOrdinal("codigoPago")) ? null : reader.GetString(reader.GetOrdinal("codigoPago"));
                    item.NombreCliente = reader.IsDBNull(reader.GetOrdinal("nombreCliente")) ? null : reader.GetString(reader.GetOrdinal("nombreCliente"));
                    item.Correo = reader.IsDBNull(reader.GetOrdinal("correo")) ? null : reader.GetString(reader.GetOrdinal("correo"));
                    item.Telefono = reader.IsDBNull(reader.GetOrdinal("cl_numeroTelefono")) ? null : reader.GetString(reader.GetOrdinal("cl_numeroTelefono"));
                    item.TipoTramite = reader.IsDBNull(reader.GetOrdinal("tipoTramite")) ? null : reader.GetString(reader.GetOrdinal("tipoTramite"));
                    item.NombreTramite = reader.IsDBNull(reader.GetOrdinal("nombreTramite")) ? null : reader.GetString(reader.GetOrdinal("nombreTramite"));
                    item.PagoFecha = reader.IsDBNull(reader.GetOrdinal("fecha")) ? null : reader.GetDateTime(reader.GetOrdinal("fecha"));
                    item.TrId = reader.GetInt32(reader.GetOrdinal("tr_id"));
                    item.StatusEvaluacion = reader.IsDBNull(reader.GetOrdinal("StatusEvaluacion")) ? "Pendiente" : reader.GetString(reader.GetOrdinal("StatusEvaluacion"));
                    item.estadoProceso =  reader.GetInt32(reader.GetOrdinal("estadoProceso"));
                    item.Doctor =  reader.GetString(reader.GetOrdinal("Doctor"));

                    lista.Add(item);
                }
                reader.Close();
                return lista;
            });
        }
    }
}
