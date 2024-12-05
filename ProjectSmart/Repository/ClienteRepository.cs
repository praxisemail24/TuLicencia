using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using iText.Kernel.Pdf.Canvas.Wmf;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartLicencia.Data;
using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
    {
        private readonly IConfiguration _configuration;

        public ClienteRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        protected Cliente __mapItem(SqlDataReader dr)
        {
            return new Cliente()
            {
                cl_id = Convert.ToInt32(dr["cl_id"].ToString()),
                cl_pueblo = new Pueblos { pl_id = Convert.IsDBNull(dr["pl_id"]) ? 0 : Convert.ToInt32(dr["pl_id"]) },
                cl_nombre = dr.IsDBNull(dr.GetOrdinal("cl_nombre")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_nombre")),
                cl_primerApellido = dr.IsDBNull(dr.GetOrdinal("cl_primerApellido")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_primerApellido")),
                cl_segundoApellido = dr.IsDBNull(dr.GetOrdinal("cl_segundoApellido")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_segundoApellido")),
                cl_zip = dr.IsDBNull(dr.GetOrdinal("cl_zip")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_zip")),
                cl_direccion = dr.IsDBNull(dr.GetOrdinal("cl_direccion")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_direccion")),
                cl_numeroLicencia = dr.IsDBNull(dr.GetOrdinal("cl_numeroLicencia")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_numeroLicencia")),
                cl_numeroSeguro = dr.IsDBNull(dr.GetOrdinal("cl_numeroSeguro")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_numeroSeguro")),
                cl_fechaNacimiento = dr.IsDBNull(dr.GetOrdinal("cl_fechaNacimiento")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("cl_fechaNacimiento")),
                cl_numeroTelefono = dr.IsDBNull(dr.GetOrdinal("cl_numeroTelefono")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_numeroTelefono")),
                cl_correo = dr.IsDBNull(dr.GetOrdinal("cl_correo")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_correo")),
                cl_nombreUsuario = dr.IsDBNull(dr.GetOrdinal("cl_nombreUsuario")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_nombreUsuario")),
                cl_contrasena = dr.IsDBNull(dr.GetOrdinal("cl_contrasena")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_contrasena")),
                cl_fechaRegistro = dr.IsDBNull(dr.GetOrdinal("cl_fechaRegistro")) ? DateTime.MinValue : dr.GetDateTime(dr.GetOrdinal("cl_fechaRegistro")),
                cl_estado = dr.IsDBNull(dr.GetOrdinal("cl_estado")) ? 0 : Convert.ToInt32(dr.GetValue(dr.GetOrdinal("cl_estado"))),
                cl_keyTemporal = dr.IsDBNull(dr.GetOrdinal("cl_keyTemporal")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_keyTemporal")),

                cl_genero = dr.IsDBNull(dr.GetOrdinal("cl_genero")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_genero")),
                cl_talla = dr.IsDBNull(dr.GetOrdinal("cl_talla")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_talla")),
                cl_peso = dr.IsDBNull(dr.GetOrdinal("cl_peso")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_peso")),
                cl_tez = dr.IsDBNull(dr.GetOrdinal("cl_tez")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_tez")),
                cl_colorPelo = dr.IsDBNull(dr.GetOrdinal("cl_colorPelo")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_colorPelo")),
                cl_colorOjo = dr.IsDBNull(dr.GetOrdinal("cl_colorOjo")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_colorOjo")),
                cl_codigoPostal = dr.IsDBNull(dr.GetOrdinal("cl_codigoPostal")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_codigoPostal")),
                cl_puebloA = dr.IsDBNull(dr.GetOrdinal("cl_pueblo")) ? string.Empty : dr.GetString(dr.GetOrdinal("cl_pueblo")),
            };
        }

        public async Task<ResponseEntity<Cliente>> GetAllCliente()
        {
            try
            {
                ResponseEntity<Cliente> result = await GetAllData("sp_ObtenerCliente", __mapItem);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> GetClienteById(int cl_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_id"] = cl_id;
                ResponseEntity<Cliente> result = await GetData("sp_ObtenerClientePorId", parameters, __mapItem);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex) {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> GetClienteByKeyTemp(string cl_keyTemporal)
        {
            try { 
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["@cl_keyTemporal"] = cl_keyTemporal;
            ResponseEntity<Cliente> result = await GetData("sp_ObtenerClientePorKeyTemporal", parameters, (SqlDataReader dr) =>
            {
                return new Cliente()
                {
                    cl_id = Convert.ToInt32(dr["cl_id"].ToString()),
                };
            });
            return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex) {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<int>> GetFrlIdByClienteAndTramite(int cl_id, int tr_id, int pg_id)
        {
            try {  
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_id"] = cl_id;
                parameters["@tr_id"] = tr_id;
                parameters["@pg_id"] = pg_id;
                ResponseEntity<int> result = await GetData("sp_ObtenerFrlIdPorClienteTramite", parameters, (SqlDataReader dr) =>
                {
                    if (dr != null && dr.HasRows)
                    {
                        return Convert.ToInt32(dr["frl_id"]);
                    }
                    else
                    {
                        return 0;
                    }
                });

                if (!result.success)
                {

                }
                return result;
            }
            catch (Exception ex)
            {
                // Manejar cualquier otro tipo de excepción
                return new ResponseEntity<int>
                {
                    success = false,
                    message = $"Ocurrió un error inesperado: {ex.Message}",
                    item = 0 
                };
            }
        }


        public async Task<ResponseEntity<string>> GetPagoByClienteAndTramite(int cl_id, int tr_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_id"] = cl_id;
                parameters["@tr_id"] = tr_id;
                ResponseEntity<string> result = await GetData("sp_ObtenerPagoPorClienteTramite", parameters, (SqlDataReader dr) =>
                {
                    if (dr != null && dr.HasRows)
                    {
                        return dr["pg_codigo"].ToString();
                    }
                    else
                    {
                        return string.Empty;
                    }
                });

                if (string.IsNullOrEmpty(result.item))
                {
                    result.success = false;
                }
                return result;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error de base de datos: {sqlEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return null;
            }

        }


        public async Task<ResponseEntity<Cliente>> GetClienteByCorreo(string correo)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_correo"] = correo;
                ResponseEntity<Cliente> result = await GetData("sp_ObtenerClientePorCorreo", parameters, (SqlDataReader dr) =>
                {
                    return new Cliente()
                    {
                        cl_id = Convert.ToInt32(dr["cl_id"].ToString()),
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_nombreUsuario = dr["cl_nombreUsuario"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<Cliente> GetClienteByCorreo2Async(string correo)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_correo"] = correo;
                ResponseEntity<Cliente> result = await GetData("sp_ObtenerClientePorCorreo", parameters, (SqlDataReader dr) =>
                {
                    return new Cliente()
                    {
                        cl_id = Convert.ToInt32(dr["cl_id"].ToString()),
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_nombreUsuario = dr["cl_nombreUsuario"].ToString(),
                    };
                });
                return result.item;
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"Error de base de datos: {sqlEx.Message}");
                return new Cliente();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ocurrió un error inesperado: {ex.Message}");
                return new Cliente();
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        public async Task<ResponseEntity<Cliente>> GetLoginbyUsuario(string cl_nombreUsuario, string cl_contrasena)
        {
            try
            {

                // Paso 1: Obtener los datos del cliente por nombre de usuario sin validar la contraseña en la base de datos
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_nombreUsuario"] = cl_nombreUsuario;

                ResponseEntity<Cliente> result = await GetData("sp_ObtenerDatosporUsuario", parameters, __mapItem);

                // Paso 2: Verificar la contraseña utilizando BCrypt
                if (result.item != null)
                {
                    // La contraseña es válida

                    if (BCrypt.Net.BCrypt.Verify(cl_contrasena, result.item.cl_contrasena))
                    {
                        return result;
                    }

                    else
                    {
                        // La contraseña no coincide
                        return new ResponseEntity<Cliente>
                        {
                            success = false,
                            message = "Nombre de usuario o contraseña incorrectos."
                        };
                    }

                }
                else
                {
                    // La contraseña no coincide
                    return new ResponseEntity<Cliente>
                    {
                        success = false,
                        message = "Nombre de usuario o contraseña incorrectos."
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }

        }


        public async Task<ResponseEntity<Cliente>> AddCliente(Cliente cliente)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                {
                    { "pl_id", cliente.cl_pueblo.pl_id },
                    { "cl_nombre", cliente.cl_nombre },
                    { "cl_primerApellido", cliente.cl_primerApellido },
                    { "cl_segundoApellido", cliente.cl_segundoApellido },
                    { "cl_zip", cliente.cl_zip },
                    { "cl_direccion", cliente.cl_direccion },
                    { "cl_numeroLicencia", cliente.cl_numeroLicencia },
                    { "cl_numeroSeguro", cliente.cl_numeroSeguro },
                    { "cl_fechaNacimiento", cliente.cl_fechaNacimiento },
                    { "cl_numeroTelefono", cliente.cl_numeroTelefono },
                    { "cl_correo", cliente.cl_correo },
                    { "cl_nombreUsuario", string.IsNullOrWhiteSpace(cliente.cl_nombreUsuario) ? DBNull.Value : cliente.cl_nombreUsuario },
                    { "cl_contrasena", string.IsNullOrWhiteSpace(cliente.cl_contrasena) ? DBNull.Value : HashPassword(cliente.cl_contrasena)},
                    { "cl_fechaRegistro", cliente.cl_fechaRegistro },
                    { "cl_estado", cliente.cl_estado }
                };
                ResponseEntity<Cliente> result = await Add(cliente, "sp_RegistrarCliente", parameters);

                if (Convert.ToInt32(result.extra) == -1)
                {
                    result.success = false;
                    result.message = "Existe un cliente con el mismo número de seguro.";
                }
                else if (Convert.ToInt32(result.extra) == -2)
                {
                    result.success = false;
                    result.message = "Existe un cliente con el mismo correo electrónico.";
                }
                else if (Convert.ToInt32(result.extra) == -3)
                {
                    result.success = false;
                    result.message = "Existe un cliente con el mismo nombre de usuario.";
                }
                else if (Convert.ToInt32(result.extra) == -4)
                {
                    result.success = false;
                    result.message = "Existe un cliente con el mismo número de licencia.";
                }
                else if (result.success)
                {
                    result.message = "Se ha registrado correctamente.";
                }
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> UpdateCliente(Cliente cliente)
        {
            try
            {
                string primerNombre = cliente.cl_nombre.TrimEnd().TrimStart();
                string segundoNombre = string.IsNullOrWhiteSpace(cliente.cl_segundoNombre) ? string.Empty : cliente.cl_segundoNombre.TrimEnd().TrimStart();
                string nombres = $"{primerNombre} {segundoNombre}".TrimStart().TrimEnd();

                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", cliente.cl_id },
                    { "pl_id", cliente.cl_pueblo.pl_id },
                    { "cl_nombre", nombres },
                    { "cl_primerApellido", cliente.cl_primerApellido },
                    { "cl_segundoApellido", cliente.cl_segundoApellido },
                    { "cl_zip", cliente.cl_zip },
                    { "cl_direccion", cliente.cl_direccion },
                    { "cl_numeroLicencia", cliente.cl_numeroLicencia },
                    { "cl_numeroSeguro", cliente.cl_numeroSeguro },
                    { "cl_fechaNacimiento", cliente.cl_fechaNacimiento },
                    { "cl_numeroTelefono", cliente.cl_numeroTelefono },
                    { "cl_correo", cliente.cl_correo },
                    { "cl_nombreUsuario", cliente.cl_nombreUsuario },
                    { "cl_contrasena", string.IsNullOrWhiteSpace(cliente.cl_contrasena) ? DBNull.Value : HashPassword(cliente.cl_contrasena) },
                    { "cl_estado", cliente.cl_estado },
                    //{ "cl_keyTemporal", cliente.cl_keyTemporal }
                };
                ResponseEntity<Cliente> result = await Update(cliente, "sp_ActualizarCliente", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
                
        public async Task<ResponseEntity<Cliente>> UpdateClienteCambioPass(int cl_id, string newPass)
        {
            try
            {
                    var parameters = new Dictionary<string, object>
                    {
                        { "cl_id", cl_id },
                        { "cl_contrasena", HashPassword(newPass) }
                    };
                    ResponseEntity<Cliente> result = await Update(new Cliente(), "sp_ActualizarClienteCambioPass", parameters);
                    return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> UpdateClientePassword(int cl_id, string newPass, string cl_keytemporal)
        {
            try
            {

                if (string.IsNullOrEmpty(cl_keytemporal))
                {
                    return new ResponseEntity<Cliente>
                    {
                        success = false,
                        message = "No se puede actualizar la contraseña sin la clave temporal."
                    };
                }

                var existsParam = new Dictionary<string, object>
                    {           
                        { "cl_id", cl_id },
                        { "cl_keytemporal", cl_keytemporal }
                    };

                    int exists = await CheckIfClienteExists(cl_id.ToString(), cl_keytemporal);
                    if (exists == 0)
                    {
                        return new ResponseEntity<Cliente>
                        {
                            success = false,
                            message = "Clave incorrecta."
                        };
                    }


                    var parameters = new Dictionary<string, object>
                    {
                        { "cl_id", cl_id },
                        { "cl_contrasena", HashPassword(newPass) },
                        { "cl_keytemporal", cl_keytemporal }
                    };
                    ResponseEntity<Cliente> result = await Update(new Cliente(), "sp_ActualizarClientePassword", parameters);
                    return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        private async Task<int> CheckIfClienteExists(String cl_id, string cl_keytemporal)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "@cl_id", cl_id },
                { "@cl_keytemporal", cl_keytemporal }
            };

            ResponseEntity<int> result = await GetData("sp_CheckClienteExists", parameters, (SqlDataReader dr) =>
            {
                return dr["resultado"] != DBNull.Value ? Convert.ToInt32(dr["resultado"]) : 0;
            });

            return result.item; 

        }

        public async Task<ResponseEntity<Cliente>> UpdateClienteKeyTemporal(int clienteId, string keyTemporal)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "cl_id", clienteId },
                    { "cl_keyTemporal", keyTemporal }
                };
                ResponseEntity<Cliente> result = await Update(new Cliente(), "sp_ActualizarClienteKeyTemporal", parameters);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el campo cl_keyTemporal: {ex.Message}");
                return new ResponseEntity<Cliente> { success = false, message = $"Error: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> UpdateClienteUsuario(int cl_id, string newUsuario, string cl_keytemporal)
        {
            try
            {

                if (string.IsNullOrEmpty(cl_keytemporal))
                {
                    return new ResponseEntity<Cliente>
                    {
                        success = false,
                        message = "No se puede actualizar la contraseña sin la clave temporal."
                    };
                }
                var parameters = new Dictionary<string, object>
            {
                { "cl_id", cl_id },
                { "cl_nombreUsuario", newUsuario },
                { "cl_keytemporal", cl_keytemporal }
            };
                ResponseEntity<Cliente> result = await Update(new Cliente(), "sp_ActualizarClienteUsuario", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> DeleteCliente(int cl_id)
        {
            try
            {

                    var parameters = new Dictionary<string, object>
                {
                    { "cl_id", cl_id }
                };
                    ResponseEntity<Cliente> result = await DeleteEntity("sp_EliminarCliente", parameters);
                    return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Cliente>> GetFrmValidacion(int cl_id, int pg_id, int tr_id)
        {
            string connectionString = _configuration.GetConnectionString("Conexion");
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    string spName = "sp_formDataExists";
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(spName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@tr_id", tr_id);
                        command.Parameters.AddWithValue("@pg_id", pg_id);
                        command.Parameters.AddWithValue("@cl_id", cl_id);
                        command.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                        await command.ExecuteNonQueryAsync();
                        int result = Convert.ToInt32(command.Parameters["Resultado"].Value);
                        return new ResponseEntity<Cliente>
                        {
                            success = result != 0,
                            message = result != 0 ? "Existe registro" : "No hay registro"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetFrmValidacion: {ex.Message}");
                return new ResponseEntity<Cliente> { success = false, message = "Error al ejecutar el procedimiento almacenado" };
            }
        }

        public async Task<ResponseEntity<Dictionary<string, object>>> GetValidarArchivos(int tr_id, int cl_id, int frm_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@tr_id"] = tr_id;
                parameters["@cl_id"] = cl_id;
                parameters["@frm_id"] = frm_id;

                ResponseEntity<Dictionary<string, object>> result = await GetData("sp_ValidarArchivos", parameters, (SqlDataReader dr) =>
                {
                    var dictionary = new Dictionary<string, object>();
                    string columnName = dr["ConteoArchivos"].ToString();
                    object columnValue = dr["EstadoArchivos"].ToString();
                    dictionary["ke1"] = columnName;
                    dictionary["ke2"] = columnValue;
                    return dictionary;
                });
                if (Convert.ToString(result.item["ke1"]) == "0")
                {
                    result.success = false;
                }
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Dictionary<string, object>>
                {
                    success = false,
                    message = $"Error de base de datos: {sqlEx.Message}",
                    item = null 
                };
            }
            catch (Exception ex)
            {
                return new ResponseEntity<Dictionary<string, object>>
                {
                    success = false,
                    message = $"Ocurrió un error inesperado: {ex.Message}",
                    item = null 
                };
            }

        }

        public async Task<ResponseEntity<int>> GetValidarEstadoForm(int cl_id, int tr_id)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@cl_id"] = cl_id;
                parameters["@tr_id"] = tr_id;
                ResponseEntity<int> result = new ResponseEntity<int>();
                int estado = 0;
                await GetData("sp_ObtenerEstadoForm", parameters, (SqlDataReader dr) =>
                {
                    if (dr != null && dr.HasRows)
                    {
                        if (dr["estado"].ToString() != "")
                        {
                            estado = Convert.ToInt32(dr["estado"]);
                            result.extra = estado;
                            result.message = "estado: " + estado;
                        }
                        else estado = 10000;
                    }
                    return estado;
                });
                if (result.extra == null)
                    result.success = false;
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<int>
                {
                    success = false,
                    message = $"Error de base de datos: {sqlEx.Message}",
                    extra = 0
                };
            }
            catch (Exception ex)
            {
                return new ResponseEntity<int>
                {
                    success = false,
                    message = $"Ocurrió un error inesperado: {ex.Message}",
                    extra = 0
                };
            }
        }



        public async Task<ResponseEntity<Cliente>> GetBuscarClientePanel(string cl_nombre = null, string cl_primerApellido = null, string cl_segundoApellido = null, string cl_correo = null, string cl_nombreUsuario = null, string cl_numeroLicencia = null, string cl_numeroTelefono = null)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(cl_nombre))
                    parameters["@cl_nombre"] = cl_nombre;

                if (!string.IsNullOrEmpty(cl_primerApellido))
                    parameters["@cl_primerApellido"] = cl_primerApellido;

                if (!string.IsNullOrEmpty(cl_segundoApellido))
                    parameters["@cl_segundoApellido"] = cl_segundoApellido;

                if (!string.IsNullOrEmpty(cl_correo))
                    parameters["@cl_correo"] = cl_correo;

                if (!string.IsNullOrEmpty(cl_nombreUsuario))
                    parameters["@cl_nombreUsuario"] = cl_nombreUsuario;

                if (!string.IsNullOrEmpty(cl_numeroLicencia))
                    parameters["@cl_numeroLicencia"] = cl_numeroLicencia;

                if (!string.IsNullOrEmpty(cl_numeroTelefono))
                    parameters["@cl_numeroTelefono"] = cl_numeroTelefono;

                ResponseEntity<Cliente> result = await GetAllDataById("spPanel_buscarCliente", (SqlDataReader dr) =>
                {
                    return new Cliente()
                    {
                        cl_id = Convert.ToInt32(dr["cl_id"].ToString()),
                        cl_pueblo = new Pueblos
                        {
                            pl_id = Convert.IsDBNull(dr["cl_pueblo"]) ? 0 : Convert.ToInt32(dr["cl_pueblo"]),
                            pl_nombre = dr["pl_nombre"].ToString()
                        },
                        cl_nombre = dr["cl_nombre"].ToString(),
                        cl_primerApellido = dr["cl_primerApellido"].ToString(),
                        cl_segundoApellido = dr["cl_segundoApellido"].ToString(),
                        cl_zip = dr[nameof(Cliente.cl_zip)].ToString(),
                        cl_direccion = dr[nameof(Cliente.cl_direccion)].ToString(),
                        cl_numeroLicencia = dr["cl_numeroLicencia"].ToString(),
                        cl_numeroSeguro = dr["cl_numeroSeguro"].ToString(),
                        cl_fechaNacimiento = Convert.ToDateTime(dr["cl_fechaNacimiento"].ToString()),
                        cl_numeroTelefono = dr["cl_numeroTelefono"].ToString(),
                        cl_correo = dr["cl_correo"].ToString(),
                        cl_nombreUsuario = dr["cl_nombreUsuario"].ToString(),
                        cl_contrasena = dr["cl_contrasena"].ToString(),
                        cl_fechaRegistro = Convert.ToDateTime(dr["cl_fechaRegistro"].ToString()),
                        cl_estado = Convert.ToInt32(dr["cl_estado"].ToString()),
                        cl_keyTemporal = dr["cl_keyTemporal"].ToString(),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Cliente> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<DetallePago>>GetDetallePago(string codigoPago)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(codigoPago))
                    parameters["@codigoPago"] = codigoPago;

                ResponseEntity<DetallePago> result = await GetAllDataById("GetDetallePago", (SqlDataReader dr) =>
                {
                    return new DetallePago()
                    {
                        nombre = dr["nombre"].ToString(),
                        codigoPago = dr["codigoPago"].ToString(),
                        fecha = dr["fecha"].ToString(),
                        tramite = dr["tramite"].ToString(),
                        tramitetabla = dr["tramitetabla"].ToString(),
                        monto = dr["monto"].ToString(),
                        subtotal = dr["subtotal"].ToString(),
                        total = dr["total"].ToString(),
                        telefono = dr["telefono"].ToString(),
                        direccion = dr["direccion"].ToString(),
                        transaccion = dr["transaccion"].ToString(),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<DetallePago> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<DetallePago> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<IEnumerable<Cliente>> Search(string column, string search)
        {
            string query = $"SELECT * FROM Cliente WHERE {column} LIKE '%{search}%'";
            var rs = await GetAllQuery("sp_ObtenerCliente", __mapItem);

            if (!rs.success)
                return new List<Cliente>();

            return rs.items;
        }
    }

}
