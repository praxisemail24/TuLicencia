using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly IPagoRepository _pagoRepository;

        public DoctorController(IDoctorRepository doctorRepository, IPagoRepository pagoRepository)
        {
            _doctorRepository = doctorRepository;
            _pagoRepository = pagoRepository;
        }

        // Método para obtener los casos asignados a un doctor con filtros
        [HttpPost("ObtenerCasosAsignados")]
        public ActionResult<ResponseEntity<List<CasoAsignado>>> ObtenerCasosAsignados([FromBody] FiltroCasosRequest filtro)
        {
            ResponseEntity<List<CasoAsignado>> response = new ResponseEntity<List<CasoAsignado>>();

            if (filtro.DoctorId <= 0)
            {
                response.success = false;
                response.message = "Doctor ID inválido.";
                return BadRequest(response);
            }

            var casos = _doctorRepository.ObtenerCasosPorDoctorYFiltros(filtro);

            response.item = casos;
            response.success = true;

            return Ok(response);
        }

        [HttpPost("ObtenerCasosAsignadosTable")]
        public DataTableJS<CasoAsignado> ObtenerCasosAsignadosTable([FromForm] DataTableJSRequest<FiltroCasosRequest> request)
        {
            DataTableJS<CasoAsignado> response = new DataTableJS<CasoAsignado>();
            try
            {
                var filtro = request.AdditionalValues;

                if (filtro.DoctorId <= 0)
                    throw new Exception("Doctor ID inválido.");

                var casos = _doctorRepository.ObtenerCasosPorDoctorYFiltros(filtro);
                var dataPage = casos.Skip(request.Start).Take(request.Length);

                response.Draw = request.Draw;
                response.Data = dataPage;
                response.RecordsTotal = casos.Count;
                response.RecordsFiltered = casos.Count();
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }


        [HttpGet("GetEvaluacion/{id}/{tr_id}")]
        public async Task<IActionResult> GetEvaluacion(int id,int tr_id)
        {
            // Lógica para obtener la evaluación por ID
            var evaluacion = await _doctorRepository.ObtenerEvaluacionPorId(id, tr_id);
            if (evaluacion != null)
            {
                return Ok(new { success = true, item = evaluacion });
            }
            return NotFound(new { success = false, message = "Evaluación no encontrada" });
        }

        [HttpPost("GuardarEvaluacion")]
        public async Task<IActionResult> GuardarEvaluacion([FromBody] EvaluacionRequest request)
        {
            try
            {
                // Lógica para guardar la evaluación
                await _doctorRepository.GuardarEvaluacion(request);


                if (request.Estado == "1")
                {
                    await _pagoRepository.GenerarEvaluacionMedicaPDF(request.Id.ToString(),request.tr_id.ToString(), $"evaluacion_{request.Id}_{request.tr_id}.pdf", request);
                }
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    } 
}