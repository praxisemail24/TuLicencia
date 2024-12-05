using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public async Task<IActionResult> SendConfirmacionArchivos(int cl_id)
        {
            ResponseEntity<Cliente> clienteResponse = await _clienteRepository.GetClienteById(cl_id);
            Cliente cliente = clienteResponse.item;
            _emailRepository.EmailConfirmacionArchivos(cliente);            
            return Ok();
        }
    }
}
