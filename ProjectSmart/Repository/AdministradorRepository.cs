using System.Data;
using System.Data.SqlClient;
using Org.BouncyCastle.Crypto.Generators;
using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public class AdministradorRepository : BaseRepository<Administrador>, IAdministradorRepository
    {
        private readonly IConfiguration _configuration;

        public AdministradorRepository(IConfiguration configuration) : base(configuration)
        {
            _configuration = configuration;
        }

        public async Task<ResponseEntity<Administrador>> GetAllAdministrador()
        {
            try
            {
            
                ResponseEntity<Administrador> result = await GetAllData("sp_ObtenerAdministrador", (SqlDataReader dr) =>
                {
                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = dr["adm_clv"].ToString(),
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Administrador>> GetAdministradorById(int adm_id)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@adm_id"] = adm_id;
                ResponseEntity<Administrador> result = await GetData("sp_ObtenerAdministradorPorId", parameters, (SqlDataReader dr) =>
                {
                    var relativePath = dr["firma"].ToString();
                    var fullPath = Path.Combine("https://api.tulicenciapr.com/", relativePath);


                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = dr["adm_clv"].ToString(),
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                        adm_firma = fullPath,
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Administrador>> GetLogin(string adm_user, string adm_clv)
        {
            try
            {
            
                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@adm_user"] = adm_user;
                parameters["@adm_clv"] = adm_clv;
                ResponseEntity<Administrador> result = await GetData("sp_ObtenerAdministradorByLogin", parameters, (SqlDataReader dr) =>
                {
                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = dr["adm_clv"].ToString(),
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Administrador>> GetLogin(LoginModel Login)
        {
            try
            {

                Dictionary<string, object> parameters = new Dictionary<string, object>();
                parameters["@adm_user"] = Login.adm_user;
                parameters["@adm_clv"] = Login.adm_clv;
                ResponseEntity<Administrador> result = await GetData("sp_ObtenerAdministradorByLogin", parameters, (SqlDataReader dr) =>

                {
                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = HashPassword(dr["adm_clv"].ToString()),  // El hash de la contraseña almacenada
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public async Task<ResponseEntity<Administrador>> AddAdministrador(Administrador administrador)
        {
            try
            { 

                administrador.adm_fech_reg = DateTime.Now;

                var parameters = new Dictionary<string, object>
                {
                    { "adm_user", administrador.adm_user },
                    //{ "adm_clv", HashPassword(administrador.adm_clv) },
                    { "adm_clv", administrador.adm_clv },
                    { "adm_nombres", administrador.adm_nombres },
                    { "adm_est", administrador.adm_est },
                    { "adm_nivel", administrador.adm_nivel },
                    { "adm_fech_reg", administrador.adm_fech_reg },
                    { "adm_email", administrador.adm_email },
                    { "adm_firma", administrador.adm_firma },
                };
                ResponseEntity<Administrador> result = await Add(administrador, "sp_RegistrarAdministrador", parameters);
                return result;

            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Administrador>> UpdateAdministrador(Administrador administrador)
        {
            try
            {

                var parameters = new Dictionary<string, object>
                {
                    { "adm_id", administrador.adm_id },
                    { "adm_user", administrador.adm_user },
                    //{ "adm_clv",HashPassword(administrador.adm_clv) },
                    { "adm_clv", string.IsNullOrWhiteSpace(administrador.adm_clv) ? DBNull.Value : administrador.adm_clv },
                    { "adm_nombres", administrador.adm_nombres },
                    { "adm_est", administrador.adm_est },
                    { "adm_nivel", administrador.adm_nivel },
                    { "adm_fech_reg", administrador.adm_fech_reg },
                    { "adm_email", administrador.adm_email },
                    { "adm_firma", administrador.adm_firma },

                };
                ResponseEntity<Administrador> result = await Update(administrador, "sp_ActualizarAdministrador", parameters);
                return result;

            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Administrador>> DeleteAdministrador(int adm_id)
        {
            try 
            { 
                var parameters = new Dictionary<string, object>
                {
                    { "adm_id", adm_id }
                };
                ResponseEntity<Administrador> result = await DeleteEntity("sp_EliminarAdministrador", parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public async Task<ResponseEntity<Administrador>> GetAllAdministradorRadicadores()
        {
            try 
            { 
                ResponseEntity<Administrador> result = await GetAllData("sp_ObtenerRadicadores", (SqlDataReader dr) =>
                {
                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = dr["adm_clv"].ToString(),
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }
        public async Task<ResponseEntity<Administrador>> GetAllAdministradorDoctores()
        {
            try 
            { 
                ResponseEntity<Administrador> result = await GetAllData("sp_ObtenerDoctores", (SqlDataReader dr) =>
                {
                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = dr["adm_clv"].ToString(),
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                    };
                });
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }


        public async Task<ResponseEntity<Administrador>> GetBuscarAdminPanel(string adm_user, string adm_email, int? adm_nivel, int? adm_est)
        {
            try 
            { 
                Dictionary<string, object> parameters = new Dictionary<string, object>();

                if (!string.IsNullOrEmpty(adm_user))
                    parameters["@adm_user"] = adm_user;

                if (!string.IsNullOrEmpty(adm_email))
                    parameters["@adm_email"] = adm_email;

                if (adm_nivel.HasValue)
                    parameters["@adm_nivel"] = adm_nivel.Value;

                if (adm_est.HasValue)
                    parameters["@adm_est"] = adm_est.Value;

                ResponseEntity<Administrador> result = await GetAllDataById("spPanel_buscarAdministrador", (SqlDataReader dr) =>
                {
                    return new Administrador()
                    {
                        adm_id = Convert.ToInt32(dr["adm_id"].ToString()),
                        adm_user = dr["adm_user"].ToString(),
                        adm_clv = dr["adm_clv"].ToString(),
                        adm_nombres = dr["adm_nombres"].ToString(),
                        adm_est = Convert.ToInt32(dr["adm_est"].ToString()),
                        adm_nivel = Convert.ToInt32(dr["adm_nivel"].ToString()),
                        adm_fech_reg = Convert.ToDateTime(dr["adm_fech_reg"].ToString()),
                        adm_email = dr["adm_email"].ToString(),
                    };
                }, parameters);
                return result;
            }
            catch (SqlException sqlEx)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Error de base de datos: {sqlEx.Message}" };

            }
            catch (Exception ex)
            {
                return new ResponseEntity<Administrador> { success = false, message = $"Ocurrió un error inesperado: {ex.Message}" };
            }
        }

        public bool ChangePassword(LoginChangePasswordModel model)
        {
            return ExecProcedure<SqlConnection, SqlCommand, bool>("sp_ChangePasswordAdministrator", cmd =>
            {
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@id", model.Id));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@pwd",string.IsNullOrWhiteSpace(model.Password) ? DBNull.Value : model.Password));
                cmd.Parameters.Add(CreateParameter<SqlParameter>("@repeatPwd", string.IsNullOrWhiteSpace(model.RepeatPassword) ? DBNull.Value : model.RepeatPassword));

                return cmd.ExecuteNonQuery() > 0;
            });
        }
    }
}
