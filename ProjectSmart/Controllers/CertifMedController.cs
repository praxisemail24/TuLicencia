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
    public class CertifMedController : ControllerBase
    {
        private readonly ICertifMedRepository _certifMedRepository;

        public CertifMedController(ICertifMedRepository certifMedRepository)
        {
            _certifMedRepository = certifMedRepository;
        }


        [HttpGet]
        public async Task<ActionResult<ResponseEntity<CertifMed>>> GetCertifMed()
        {
            ResponseEntity<CertifMed> response = new ResponseEntity<CertifMed>();
            response = await _certifMedRepository.GetAllCertifMed();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEntity<CertifMed>>> GetCertifMede(int id)
        {
            ResponseEntity<CertifMed> response = new ResponseEntity<CertifMed>();
            response = await _certifMedRepository.GetCertifMedById(id);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<CertifMed>>> PostCertifMed(CertifMed certifMed)
        {
            ResponseEntity<CertifMed> response = new ResponseEntity<CertifMed>();
            response = await _certifMedRepository.AddCertifMed(certifMed);
            return response;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseEntity<CertifMed>>> PutRenovLi(CertifMed certifMed)
        {
            ResponseEntity<CertifMed> response = new ResponseEntity<CertifMed>();
            response = await _certifMedRepository.UpdateCertifMed(certifMed);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<CertifMed>>> DeleteCertifMed(int id)
        {
            ResponseEntity<CertifMed> response = new ResponseEntity<CertifMed>();
            response = await _certifMedRepository.DeleteCertifMed(id);
            return response;
        }

    }
}
