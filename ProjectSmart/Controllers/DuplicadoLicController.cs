using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DuplicadoLicController : ControllerBase
    {
        private readonly IDuplicadoLicRepository _duplicadoLicRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailRepository _emailRepository;

        public DuplicadoLicController(IDuplicadoLicRepository duplicadoLicRepository, IEmailRepository er, IClienteRepository cR)
        {
            _duplicadoLicRepository = duplicadoLicRepository;
            _emailRepository = er;
            _clienteRepository = cR;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<DuplicadoLic>>> GetDuplicadoLic()
        {
            ResponseEntity<DuplicadoLic> response = new ResponseEntity<DuplicadoLic>();
            response = await _duplicadoLicRepository.GetAllDuplicadoLic();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEntity<DuplicadoLic>>> GetDuplicadoLice(int id)
        {
            ResponseEntity<DuplicadoLic> response = new ResponseEntity<DuplicadoLic>();
            response = await _duplicadoLicRepository.GetDuplicadoLicById(id);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<DuplicadoLic>>> PostDuplicadoLic(DuplicadoLic duplicadoLic)
        {
            ResponseEntity<DuplicadoLic> response = new ResponseEntity<DuplicadoLic>();
            response = await _duplicadoLicRepository.AddDuplicadoLic(duplicadoLic);
            string nombreForm = "Duplicado de Licencia";
            if (response.success)
            {
                ResponseEntity<Cliente> cliente = await _clienteRepository.GetClienteById(duplicadoLic.cl_cliente.cl_id);
                _emailRepository.EmailConfirmacionFormulario(cliente.item, nombreForm);
            }
            return response;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseEntity<DuplicadoLic>>> PutRenovLi(DuplicadoLic duplicadoLic)
        {
            ResponseEntity<DuplicadoLic> response = new ResponseEntity<DuplicadoLic>();
            response = await _duplicadoLicRepository.UpdateDuplicadoLic(duplicadoLic);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<DuplicadoLic>>> DeleteDuplicadoLic(int id)
        {
            ResponseEntity<DuplicadoLic> response = new ResponseEntity<DuplicadoLic>();
            response = await _duplicadoLicRepository.DeleteDuplicadoLic(id);
            return response;
        }
    }
}
