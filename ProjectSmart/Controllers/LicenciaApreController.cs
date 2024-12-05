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
    public class LicenciaApreController : ControllerBase
    {
        private readonly ILicenciaApreRepository _licenciaApreRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailRepository _emailRepository;

        public LicenciaApreController(ILicenciaApreRepository licenciaApreRepository, IEmailRepository er, IClienteRepository cR)
        {
            _licenciaApreRepository = licenciaApreRepository;
            _emailRepository = er;
            _clienteRepository = cR;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<LicenciaApre>>> GetLicenciaApre()
        {
            ResponseEntity<LicenciaApre> response = new ResponseEntity<LicenciaApre>();
            response = await _licenciaApreRepository.GetAllLicenciaApre();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEntity<LicenciaApre>>> GetLicenciaApren(int id)
        {
            ResponseEntity<LicenciaApre> response = new ResponseEntity<LicenciaApre>();
            response = await _licenciaApreRepository.GetLicenciaApreById(id);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<LicenciaApre>>> PostLicenciaApre(LicenciaApre licenciaApre)
        {
            ResponseEntity<LicenciaApre> response = new ResponseEntity<LicenciaApre>();
            response = await _licenciaApreRepository.AddLicenciaApre(licenciaApre);
            string nombreForm = "Licencia de Aprendizaje";
            if (response.success)
            {
                ResponseEntity<Cliente> cliente = await _clienteRepository.GetClienteById(licenciaApre.cl_cliente.cl_id);
                _emailRepository.EmailConfirmacionFormulario(cliente.item, nombreForm);
            }
            return response;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseEntity<LicenciaApre>>> PutRenovLi(LicenciaApre licenciaApre)
        {
            ResponseEntity<LicenciaApre> response = new ResponseEntity<LicenciaApre>();
            response = await _licenciaApreRepository.UpdateLicenciaApre(licenciaApre);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<LicenciaApre>>> DeleteLicenciaApre(int id)
        {
            ResponseEntity<LicenciaApre> response = new ResponseEntity<LicenciaApre>();
            response = await _licenciaApreRepository.DeleteLicenciaApre(id);
            return response;
        }

    }
}
