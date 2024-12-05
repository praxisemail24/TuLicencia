using SmartLicencia.Data;
using SmartLicencia.Models;
using System.Data;
using System.Data.SqlClient;


namespace SmartLicencia.Repository
{
    public class UsuarioRepository : AbstractConnection, IUsuarioRepository
    {
        private readonly IConfiguration _configuration;

        public UsuarioRepository(IConfiguration configuration) :base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Usuario>> GetAllUsuarios()
        {
            List<Usuario> rpt = new List<Usuario>();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerUsuario", oConexion);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        rpt.Add(new Usuario()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            Name = dr["Name"].ToString(),
                            Email = dr["Email"].ToString()                            
                        });
                    }
                    dr.Close();

                    return rpt;

                }
                catch (Exception ex)
                {
                    rpt = null;
                    return rpt;
                }
            }
        }


        public async Task<Usuario> GetUsuarioById(int id)
        {
            Usuario rpt = new Usuario();
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection oConexion = new SqlConnection(conexion))
            {
                SqlCommand cmd = new SqlCommand("ObtenerUsuarioPorId", oConexion);
                cmd.Parameters.AddWithValue("Id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        rpt.Id = Convert.ToInt32(dr["Id"].ToString());
                        rpt.Name = dr["Name"].ToString();
                        rpt.Email = dr["Email"].ToString();                    
                    }
                    dr.Close();
                    return rpt;

                }
                catch (Exception ex)
                {
                    rpt = null;
                    return rpt;
                }
            }
        }

        public async Task AddUsuario(Usuario usuario)
        {
            int respuesta = 0;
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("Name", usuario.Name);
                    cmd.Parameters.AddWithValue("Email", usuario.Email);
                    cmd.Parameters.AddWithValue("Password", usuario.Password);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
        }

        public async Task UpdateUsuario(Usuario usuario)
        {
            int respuesta = 0;
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("ActualizarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("Id", usuario.Id);
                    cmd.Parameters.AddWithValue("Name", usuario.Name);
                    cmd.Parameters.AddWithValue("Email", usuario.Email);
                    cmd.Parameters.AddWithValue("Password", usuario.Password);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
        }

        public async Task DeleteUsuario(int id)
        {
            int respuesta = 0;
            string conexion = _configuration.GetConnectionString("Conexion");
            using (SqlConnection oconexion = new SqlConnection(conexion))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("EliminarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("Id", id);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;
                    oconexion.Open();
                    cmd.ExecuteNonQuery();
                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                }
                catch (Exception ex)
                {
                    respuesta = 0;
                }
            }
        }

        public IEnumerable<string> ListarCorreosUsuario(string query)
        {
            return ExecProcedure<SqlConnection, SqlCommand, IEnumerable<string>>("sp_ListarCorreos", cmd =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@buscar", query));
                var lista = new List<string>();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(reader.IsDBNull(reader.GetOrdinal("correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("correo")));
                }
                reader.Close();
                return lista;
            });
        }
    }
}
