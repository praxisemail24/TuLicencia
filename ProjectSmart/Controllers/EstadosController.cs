using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EstadosController : ControllerBase
    {
        private readonly IEstadosRepository _estadosRepository;

        public EstadosController(IEstadosRepository estadosRepository)
        {
            _estadosRepository = estadosRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estados>>> GetEstados()
        {
            try
            {
                var estados = await _estadosRepository.GetAllEstados();
                return Ok(estados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<Pueblos>> GetUsuario(int id)
        //{
        //    try
        //    {
        //        var pueblo = await _pueblosRepository.GetPueblosById(id);
        //        if (pueblo == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(pueblo);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        //    }
        //}

    }
}
