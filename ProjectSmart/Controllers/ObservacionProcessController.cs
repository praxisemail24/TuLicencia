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
    public class ObservacionProcessController : ControllerBase
    {
        private readonly IObservacionProcessRepository _observacionProcessRepository;

        public ObservacionProcessController(IObservacionProcessRepository observacionProcessRepository)
        {
            _observacionProcessRepository = observacionProcessRepository;
        }


        [HttpGet("{ob_id}")]
        public async Task<ActionResult<ResponseEntity<ObservacionProcess>>> GetObservacionProcess(int ob_id)
        {
            ResponseEntity<ObservacionProcess> response = new ResponseEntity<ObservacionProcess>();
            response = await _observacionProcessRepository.GetObservacionProcessById(ob_id);
            return response;
        }


        [HttpPost]
        public async Task<ActionResult<ResponseEntity<ObservacionProcess>>> PostObservacionProcess(ObservacionProcess observacionProcess)
        {
            ResponseEntity<ObservacionProcess> response = new ResponseEntity<ObservacionProcess>();
            response = await _observacionProcessRepository.AddObservacionProcess(observacionProcess);
            return response;
        }



    }
}

    
