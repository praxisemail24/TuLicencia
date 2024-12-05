using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class PueblosController : ControllerBase
    {
        private readonly IPueblosRepository _pueblosRepository;

        public PueblosController(IPueblosRepository pueblosRepository)
        {
            _pueblosRepository = pueblosRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pueblos>>> GetPueblos()
        {
            try
            {
                var pueblos = await _pueblosRepository.GetAllPueblos();
                return Ok(pueblos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Pueblos>> GetUsuario(int id)
        {
            try
            {
                var pueblo = await _pueblosRepository.GetPueblosById(id);
                if (pueblo == null)
                {
                    return NotFound();
                }
                return Ok(pueblo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

    }
}
