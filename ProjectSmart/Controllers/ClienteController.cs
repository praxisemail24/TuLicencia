using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Utility;


namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IConfiguration _configuration;

        public ClienteController(IClienteRepository clienteRepository, IEmailRepository er, IConfiguration configuration)
        {
            _clienteRepository = clienteRepository;
            _emailRepository = er;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetCliente()
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetAllCliente();
            return response;
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetCliente(int id)
        {
            try {
                ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
                response = await _clienteRepository.GetClienteById(id);
                return response;
            }
            catch(Exception ex) {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "Ocurrió un error al procesar la solicitud.", Error = ex.Message });
            }

        }


        [HttpGet("Frl/{cl_id}/{tr_id}/{pg_id}")]
        public async Task<ActionResult<ResponseEntity<int>>> GetFrlId(int cl_id, int tr_id, int pg_id)
        {
            ResponseEntity<int> response = await _clienteRepository.GetFrlIdByClienteAndTramite(cl_id, tr_id, pg_id);
            return response;
        }


        [HttpGet("PagoValidacion/{cl_id}/{tr_id}")]
        public async Task<ActionResult<ResponseEntity<string>>> GetPago(int cl_id, int tr_id)
        {
            ResponseEntity<string> response = await _clienteRepository.GetPagoByClienteAndTramite(cl_id, tr_id);
            return response;
        }

        [HttpGet("ClienteKey/{cl_keyTemporal}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetClienteByKeyTemp(string cl_keyTemporal)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetClienteByKeyTemp(cl_keyTemporal);
            return response;
        }

        [HttpGet("DatosXCorreo/{correo}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetClienteCorreo(string correo)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetClienteByCorreo(correo);
            return response;
        }

        [HttpGet("login/{cl_nombreUsuario}/{cl_contrasena}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetLogin(string cl_nombreUsuario, string cl_contrasena)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetLoginbyUsuario(cl_nombreUsuario, cl_contrasena);
            if(response.success)
            {
                response.item.token = TokenGenerator.GenerateJwtToken(_configuration, cl_nombreUsuario);
            }
            return response;
        }


        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> PostLogin(LoginModelCliente login)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetLoginbyUsuario(login.cl_nombreUsuario, login.cl_contrasena);
            if (response.success)
            {
                response.item.token = TokenGenerator.GenerateJwtToken(_configuration, login.cl_nombreUsuario);
            }
            return response;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> PostCliente(Cliente cliente)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.AddCliente(cliente);

            if (response.success)
            {
                _emailRepository.EmailRegistro(cliente);
            }
            return response;
        }

        [HttpPut]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> PutCliente(Cliente cliente)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.UpdateCliente(cliente);
            return response;
        }


        [HttpPut("CambioPassCliente/{cl_id}/{newPass}")]
        public async Task<ActionResult<ResponseEntity<Cliente>>> PutClienteCambioPass(int cl_id, string newPass)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.UpdateClienteCambioPass(cl_id, newPass);
            return response;
        }
        
        [HttpPut("Actualizar/{cl_id}/{newPass}/{keyTemporal}")]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Cliente>>> PutClientePass(int cl_id, string newPass, string keyTemporal)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.UpdateClientePassword(cl_id, newPass, keyTemporal);

            if (response.success)
            {
                ResponseEntity<Cliente> cliente = await _clienteRepository.GetClienteById(cl_id);
                _emailRepository.EmailConfirmacionCambioPass(cliente.item);
            }
            return response;
        }
        

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<Cliente>>> DeleteCliente(int id)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.DeleteCliente(id);
            return response;
        }

        [HttpGet("{cl_id}/{pg_id}/{tr_id}")]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetClienteFrmValidacion(int cl_id, int pg_id, int tr_id)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetFrmValidacion(cl_id, pg_id, tr_id);
            return response;
        }

        [HttpGet("Archivo/{tr_id}/{cl_id}/{frm_id}")]
        public async Task<ActionResult<ResponseEntity<Dictionary<string, object>>>> GetValidacionArchivos(int tr_id, int cl_id, int frm_id)
        {
           ResponseEntity<Dictionary<string, object>> response = new ResponseEntity<Dictionary<string, object>>();
            response = await _clienteRepository.GetValidarArchivos(tr_id, cl_id, frm_id);
            return response;
        }


        [HttpGet("validacionEstadoForm/{cl_id}/{tr_id}")]
        public async Task<ActionResult<ResponseEntity<int>>> GetValidacionPagoxEstado(int cl_id, int tr_id)
        {
            ResponseEntity<int> response = await _clienteRepository.GetValidarEstadoForm(cl_id, tr_id);
            return response;
        }



        [HttpGet("buscarClientePanel")]
        public async Task<ActionResult<ResponseEntity<Cliente>>> GetClientePanel([FromQuery] string cl_nombre = null, [FromQuery] string cl_primerApellido = null, [FromQuery] string cl_segundoApellido = null, [FromQuery] string cl_correo = null, [FromQuery] string cl_nombreUsuario = null, [FromQuery] string cl_numeroLicencia = null, [FromQuery] string cl_numeroTelefono = null)
        {
            ResponseEntity<Cliente> response = new ResponseEntity<Cliente>();
            response = await _clienteRepository.GetBuscarClientePanel(cl_nombre, cl_primerApellido, cl_segundoApellido, cl_correo, cl_nombreUsuario, cl_numeroLicencia, cl_numeroTelefono);
            return response;
        }


    }
}
