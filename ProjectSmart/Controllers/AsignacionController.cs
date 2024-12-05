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
    public class AsignacionController : ControllerBase
    {
        private readonly IAsignacionRepository _asignacionRepository;

        public AsignacionController(IAsignacionRepository asignacionRepository)
        {
            _asignacionRepository = asignacionRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<Asignacion>>> PostAsignacion(Asignacion asignacion)
        {
            ResponseEntity<Asignacion> response = new ResponseEntity<Asignacion>();
            response = await _asignacionRepository.AddAsignacion(asignacion);
            return response;
        }

        [HttpPost("Doctor")]
        public async Task<ActionResult<ResponseEntity<Asignacion>>> PostAsignacionDoctor(Asignacion asignacion)
        {
            ResponseEntity<Asignacion> response = new ResponseEntity<Asignacion>();
            response = await _asignacionRepository.AddAsignacionDoctor(asignacion);
            return response;
        }

        [HttpGet("ObtenerAsignacion/{frm_id}/{tr_id}")]
        public async Task<ActionResult<ResponseEntity<Asignacion>>> GetObtenerAsignacion(int frm_id, int tr_id)
        {
            ResponseEntity<Asignacion> response = new ResponseEntity<Asignacion>();
            response = await _asignacionRepository.GetDatosAsignacion(frm_id, tr_id);
            return response;
        }

        [HttpGet("ObtenerLineaTiempo/{form_id}/{tr_id}")]
        public async Task<ResponseJSON> ObtenerLineaTiempo(int form_id, int tr_id)
        {
            var response = new ResponseJSON();
            try
            {
                var data = await _asignacionRepository.ObtenerLineaTiempo(form_id, tr_id);
                response.Success = true;
                response.Data = data;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
