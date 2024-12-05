using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MotivoController : ControllerBase
    {
        private readonly IMotivoRepository _motivoRepository;

        public MotivoController(IMotivoRepository motivoRepository)
        {
            _motivoRepository = motivoRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Motivo>>> GetMotivos()
        {
            try
            {
                var motivo = await _motivoRepository.GetAllMotivo();
                return Ok(motivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Motivo>> GetMotivo(int id)
        {
            try
            {
                var motivo = await _motivoRepository.GetMotivoById(id);
                if (motivo == null)
                {
                    return NotFound();
                }
                return Ok(motivo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
