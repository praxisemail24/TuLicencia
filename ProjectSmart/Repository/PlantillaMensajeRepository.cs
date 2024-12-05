using SmartLicencia.Data;
using SmartLicencia.Models;
using System.Data.SqlClient;

namespace SmartLicencia.Repository
{
    public class PlantillaMensajeRepository : AbstractConnection
    {
        public PlantillaMensajeRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<PlantillaMensaje> Listar(string tipo = "")
        {
            string query = "SELECT * FROM PlantillaMensaje";

            if (!string.IsNullOrWhiteSpace(tipo))
                query += $" WHERE TIPO = '{tipo}'";

            return ExecQuery<SqlConnection, SqlCommand, IEnumerable<PlantillaMensaje>>(query, (cmd) =>
            {
                var reader = cmd.ExecuteReader();
                List<PlantillaMensaje> lista = new List<PlantillaMensaje>();
                while (reader.Read())
                {
                    PlantillaMensaje pl = new PlantillaMensaje();
                    pl.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                    pl.CodigoInterno = reader.GetString(reader.GetOrdinal("CODIGO_INTERNO"));
                    pl.Cc = reader.IsDBNull(reader.GetOrdinal("CC")) ? string.Empty : reader.GetString(reader.GetOrdinal("CC"));
                    pl.Asunto = reader.IsDBNull(reader.GetOrdinal("ASUNTO")) ? string.Empty : reader.GetString(reader.GetOrdinal("ASUNTO"));
                    pl.Contenido = reader.IsDBNull(reader.GetOrdinal("CONTENIDO")) ? string.Empty : reader.GetString(reader.GetOrdinal("CONTENIDO"));
                    lista.Add(pl);
                }
                reader.Close();
                return lista;
            });
        }

        public PlantillaMensaje? ObtenerPorNombre(string codigo, string tipo = "email")
        {
            string query = $"SELECT * FROM PlantillaMensaje WHERE TIPO = '{tipo}' AND CODIGO_INTERNO = '{codigo}'";

            return ExecQuery<SqlConnection, SqlCommand, PlantillaMensaje?>(query, (cmd) =>
            {
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    PlantillaMensaje pl = new PlantillaMensaje();
                    pl.Id = reader.GetInt32(reader.GetOrdinal("ID"));
                    pl.CodigoInterno = reader.GetString(reader.GetOrdinal("CODIGO_INTERNO"));
                    pl.Cc = reader.IsDBNull(reader.GetOrdinal("CC")) ? string.Empty : reader.GetString(reader.GetOrdinal("CC"));
                    pl.Asunto = reader.IsDBNull(reader.GetOrdinal("ASUNTO")) ? string.Empty : reader.GetString(reader.GetOrdinal("ASUNTO"));
                    pl.Contenido = reader.IsDBNull(reader.GetOrdinal("CONTENIDO")) ? string.Empty : reader.GetString(reader.GetOrdinal("CONTENIDO"));
                    reader.Close();
                    return pl;
                }
                return null;
            });
        }
    }
}
