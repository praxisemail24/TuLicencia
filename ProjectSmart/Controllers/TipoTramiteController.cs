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
    public class TipoTramiteController : ControllerBase
    {
        private readonly ITipoTramiteRepository _tipoTramiteRepository;

        public TipoTramiteController(ITipoTramiteRepository tipoTramiteRepository)
        {
            _tipoTramiteRepository = tipoTramiteRepository;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<TipoTramite>>> GetTipoTramite()
        {
            ResponseEntity<TipoTramite> response = new ResponseEntity<TipoTramite>();
            response = await _tipoTramiteRepository.GetAllTipoTramite();
            return response;
        }
    }
}
