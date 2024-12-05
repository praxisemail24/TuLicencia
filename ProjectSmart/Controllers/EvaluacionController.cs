using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluacionController : Controller
    {
        private string _rootDirectory;
        protected readonly ICertifMedRepository _certifMedRepository;

        public EvaluacionController(IWebHostEnvironment env, ICertifMedRepository certifMedRepository)
        {
            _rootDirectory = Path.Combine(env.WebRootPath, $"Evaluaciones");
            _certifMedRepository = certifMedRepository;
        }

        [HttpPost("CheckStatus/{frmId}/{trId}")]
        public async Task<IActionResult> CheckStatus(int frmId, int trId)
        {
            var checkStatus = await _certifMedRepository.CheckCertifiedStatus(frmId, trId);

            return Ok(new { 
                success = checkStatus.success && !string.IsNullOrWhiteSpace(checkStatus.item.Estado),
                status = checkStatus.item.Estado,
                file = checkStatus.item.Path, 
                url = checkStatus.item.Url, 
            });
        }
    }
}
