using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Utility;


namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdministradorController : ControllerBase
    {
        private readonly IAdministradorRepository _administradorRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHost;

        public AdministradorController(IAdministradorRepository administradorRepository, IEmailRepository er, IConfiguration configuration, IWebHostEnvironment webHost)
        {
            _administradorRepository = administradorRepository;
            _emailRepository = er;
            _configuration = configuration;
            _webHost = webHost;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<Administrador>>> GetAdministrador()
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.GetAllAdministrador();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEntity<Administrador>>> GetAdministrador(int id)
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.GetAdministradorById(id);
            return response;
        }

        [HttpGet("loginAdm/{adm_user}/{adm_clv}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Administrador>>> GetLogin(string adm_user, string adm_clv)
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.GetLogin(adm_user, adm_clv);
            if (response.success) {
                response.item.Token = TokenGenerator.GenerateJwtToken(_configuration, response.item.adm_user);
            }
            return response;
        }


        [HttpPost("loginAdm")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Administrador>>> PostLogin([FromBody] LoginModel loginModel)
        {
            // Validar entrada
            if (string.IsNullOrEmpty(loginModel.adm_user) || string.IsNullOrEmpty(loginModel.adm_clv))
            {
                return BadRequest(new ResponseEntity<Administrador> { success = false, message = "Usuario o clave no proporcionados" });
            }

            // Obtener el administrador desde la base de datos
            ResponseEntity<Administrador> response = await _administradorRepository.GetLogin(loginModel);

            if (response.success)
            {
                // Verificar la contraseña utilizando el hash
                //bool isPasswordValid = VerifyPassword(loginModel.adm_clv, response.item.adm_clv);
                //if (isPasswordValid)
                //{
                    // Generar el token JWT
                    response.item.Token = TokenGenerator.GenerateJwtToken(_configuration, response.item.adm_user);
                    return Ok(response);
                //}
                //else
                //{
                //    return Unauthorized(new ResponseEntity<Administrador> { success = false, message = "Contraseña incorrecta" });
                //}
            }
            else
            {
                return NotFound(new ResponseEntity<Administrador> { success = false, message = "Usuario no encontrado" });
            }
        }

        private bool VerifyPassword(string enteredPassword, string storedHash)
        {
            // Utilizar BCrypt para verificar el hash
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }



        //[HttpPost]
        //public async Task<ActionResult<ResponseEntity<Administrador>>> PostAdministrador(Administrador administrador)
        //{
        //    ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
        //    response = await _administradorRepository.AddAdministrador(administrador);
        //    return response;
        //}

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<Administrador>>> PostAdministrador(Administrador administrador)
        {
            if (administrador.FirmaBytes != null && administrador.FirmaBytes.Length > 0)
            {
                byte[] firmaBytes = Convert.FromBase64String(administrador.FirmaBytes);
                string firmaPath = Path.Combine("wwwroot\\Firma", $"firma_{administrador.adm_user}.jpg");
                await System.IO.File.WriteAllBytesAsync(firmaPath, firmaBytes);
                administrador.adm_firma = Path.Combine("Firma", $"firma_{administrador.adm_user}.jpg");  // Actualizar la ruta en la base de datos

            }

            ResponseEntity<Administrador> response = await _administradorRepository.AddAdministrador(administrador);
            return Ok(response);
        }


        [HttpPut]
        public async Task<ActionResult<ResponseEntity<Administrador>>> PutAdministrador(Administrador administrador)
        {
            if (administrador.FirmaBytes != null && administrador.FirmaBytes.Length > 0)
            {
                string fileName = await Utils.CreateFile(administrador.FirmaBytes, $"firma_{administrador.adm_user}", Path.Combine(_webHost.ContentRootPath, "wwwroot", "Firma"));  // Actualizar la ruta en la base de datos
                administrador.adm_firma = $"Firma/{fileName}";
            }

            ResponseEntity<Administrador> response = await _administradorRepository.UpdateAdministrador(administrador);
            return Ok(response);
        }
        //[HttpPut]
        //public async Task<ActionResult<ResponseEntity<Administrador>>> PutAdministrador([FromBody] Administrador administrador)
        //{
        //    if (administrador.FirmaBytes != null && administrador.FirmaBytes.Length > 0)
        //    {
        //        string firmaPath = Path.Combine("Firma", $"firma_{administrador.adm_user}.jpg");
        //        await System.IO.File.WriteAllBytesAsync(firmaPath, administrador.FirmaBytes);
        //        administrador.adm_firma = firmaPath;  // Actualizar la ruta en la base de datos
        //    }

        //    ResponseEntity<Administrador> response = await _administradorRepository.UpdateAdministrador(administrador);
        //    return Ok(response);
        //}


        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<Administrador>>> DeleteAdministrador(int id)
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.DeleteAdministrador(id);
            return response;
        }


        [HttpGet("Radicadores")]
        public async Task<ActionResult<ResponseEntity<Administrador>>> GetAdministradorRadicadores()
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.GetAllAdministradorRadicadores();
            return response;
        }

        [HttpGet("Doctores")]
        public async Task<ActionResult<ResponseEntity<Administrador>>> GetAdministradorDoctores()
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.GetAllAdministradorDoctores();
            return response;
        }

        [HttpGet("buscarAdminPanel")]
        public async Task<ActionResult<ResponseEntity<Administrador>>> GetAdministradorPanel([FromQuery] string adm_user = null, [FromQuery] string adm_email = null, [FromQuery] int? adm_nivel = null, [FromQuery] int? adm_est = null)
        {
            ResponseEntity<Administrador> response = new ResponseEntity<Administrador>();
            response = await _administradorRepository.GetBuscarAdminPanel(adm_user, adm_email, adm_nivel, adm_est);
            return response;
        }

        [HttpPost("change_password")]
        public ResponseJSON ChangePassword([FromBody] LoginChangePasswordModel request)
        {
            var response = new ResponseJSON();
            try
            {
                if (request == null)
                    throw new Exception("Se requiere nueva contraseña.");

                if (string.IsNullOrWhiteSpace(request.Password) || string.IsNullOrWhiteSpace(request.RepeatPassword))
                    throw new Exception("Se requiere que ingrese nueva contraseña.");

                if (request.Password != request.RepeatPassword)
                    throw new Exception("Las contraseñas ingresadas no coinciden.");

                _administradorRepository.ChangePassword(request);

                response.Success = true;
                response.Message = "Se ha cambiado correctamente la contraseña.";
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                response.Error = ex;
            }
            return response;
        }
    }
}
