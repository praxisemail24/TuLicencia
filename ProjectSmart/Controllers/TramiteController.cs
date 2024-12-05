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
    public class TramiteController : ControllerBase
    {
        private readonly ITramiteRepository _tramiteRepository;

        public TramiteController(ITramiteRepository tramiteRepository)
        {
            _tramiteRepository = tramiteRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<ResponseEntity<Tramite>>> GetTramite()
        {
            ResponseEntity<Tramite> response = new ResponseEntity<Tramite>();
            response = await _tramiteRepository.GetAllTramite();
            return response;
        }
    }
}
