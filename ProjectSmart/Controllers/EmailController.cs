using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Controlador de correo electrónico.")]
    public class EmailController : ControllerBase
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IClienteRepository _clienteRepository;

        public EmailController(IEmailRepository emailRepository, IClienteRepository clienteRepository)
        {
            _emailRepository = emailRepository;
            _clienteRepository = clienteRepository;
        }

        [HttpPost("CambioPass")]
        [SwaggerOperation(
            Summary = "Restablecer contraseña.",
            Description = "Envia correo electrónico de restablecimiento de contraseña.",
            OperationId = "SendNewPass",
            Tags = new string[] { "Email" }
        )]
        public async Task<IActionResult> SendNewPass(Email request) 
        {
            var (success, errorMessage) = await _emailRepository.EmailNewPass(request);

            if (!success)
            {
                if (errorMessage.Contains("Usuario no encontrado"))
                {
                    return NotFound(new { message = errorMessage }); 
                }
                else
                {
                    return BadRequest(new { message = errorMessage });
                }
            }

            return Ok(new { message = "Correo de restablecimiento de contraseña enviado correctamente" });
        }

        [HttpPost("CambioUsuario")]
        [SwaggerOperation(
            Summary = "Envio de nuevo usuario.",
            Description = "Envia correo electrónico de nuevo usuario.",
            OperationId = "SendNewUsuario",
            Tags = new string[] { "Email" }
        )]
        public async Task<IActionResult> SendNewUsuario(Email request)
        {
            var (success, errorMessage) = await _emailRepository.EmailNewUsuario(request);
            if (!success)
            {
                if (errorMessage.Contains("Usuario no encontrado"))
                {
                    return NotFound(new { message = errorMessage });
                }
                else
                {
                    return BadRequest(new { message = errorMessage });
                }
            }

            return Ok(new { message = "Correo de restablecimiento de contraseña enviado correctamente" });
        }

        [HttpPost("ConfirmArchivos/{cl_id}")]
        [SwaggerOperation(
            Summary = "Confirmación de archivos.",
            Description = "Envio de correo electrónico al finalizar de subir archivo.",
            OperationId = "SendConfirmacionArchivos",
            Tags = new string[] { "Email" }
        )]
        public async Task<IActionResult> SendConfirmacionArchivos(int cl_id)
        {
            ResponseEntity<Cliente> clienteResponse = await _clienteRepository.GetClienteById(cl_id);
            Cliente cliente = clienteResponse.item;
            _emailRepository.EmailConfirmacionArchivos(cliente);            
            return Ok();
        }
    }
}
