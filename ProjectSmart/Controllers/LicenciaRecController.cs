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
    public class LicenciaRecController : ControllerBase
    {
        private readonly ILicenciaRecRepository _LicenciaRecRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailRepository _emailRepository;

        public LicenciaRecController(ILicenciaRecRepository licenciaRecRepository, IEmailRepository er, IClienteRepository cR)
        {
            _LicenciaRecRepository = licenciaRecRepository;
            _emailRepository = er;
            _clienteRepository = cR;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<LicenciaRec>>> GetLicenciaRec()
        {
            ResponseEntity<LicenciaRec> response = new ResponseEntity<LicenciaRec>();
            response = await _LicenciaRecRepository.GetAllLicenciaRec();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEntity<LicenciaRec>>> GetLicenciaRec(int id)
        {
            ResponseEntity<LicenciaRec> response = new ResponseEntity<LicenciaRec>();
            response = await _LicenciaRecRepository.GetLicenciaRecById(id);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<LicenciaRec>>> PostLicenciaRec(LicenciaRec licenciaRec)
        {
            ResponseEntity<LicenciaRec> response = new ResponseEntity<LicenciaRec>();
            response = await _LicenciaRecRepository.AddLicenciaRec(licenciaRec);
            string nombreForm = "Licencia de Reciprocidad";
            if (response.success)
            {
                ResponseEntity<Cliente> cliente = await _clienteRepository.GetClienteById(licenciaRec.cl_cliente.cl_id);
                _emailRepository.EmailConfirmacionFormulario(cliente.item, nombreForm);
            }
            return response;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseEntity<LicenciaRec>>> PutLicenciaRec(LicenciaRec licenciaRec)
        {
            ResponseEntity<LicenciaRec> response = new ResponseEntity<LicenciaRec>();
            response = await _LicenciaRecRepository.UpdateLicenciaRec(licenciaRec);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<LicenciaRec>>> DeleteLicenciaRec(int id)
        {
            ResponseEntity<LicenciaRec> response = new ResponseEntity<LicenciaRec>();
            response = await _LicenciaRecRepository.DeleteLicenciaRec(id);
            return response;
        }
    }
}
