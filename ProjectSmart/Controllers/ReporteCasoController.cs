using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Utility;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReporteCasoController : ControllerBase
    {
        private readonly ReporteCasoRepository _reporteRepository;

        public ReporteCasoController(IConfiguration config)
        {
            _reporteRepository = new ReporteCasoRepository(config);
        }

        [HttpPost("general")]
        public DataTableJS<ReporteTramiteCaso> General([FromForm] DataTableJSRequest<ReporteCasoRequest> dataTable)
        {
            var response = new DataTableJS<ReporteTramiteCaso>();
            try
            {
                var request = dataTable.AdditionalValues;

                var data = _reporteRepository.ReporteGeneral(
                    request.TipoTramite, request.EstadoTipo, request.EstadoProceso, request.Nombres,
                    request.ApellidoPaterno, request.ApellidoMaterno, request.Correo
                );

                response.Data = data;
                response.Draw = dataTable.Draw;
                response.RecordsFiltered = data.Count();
                response.RecordsTotal = data.Count();
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }

            return response;
        }

        [HttpGet("general/exportar")]
        [AllowAnonymous]
        public FileStreamResult GeneralExportar([FromQuery] ReporteCasoRequest request)
        {
            var data = _reporteRepository.ReporteGeneral(
                request.TipoTramite, request.EstadoTipo, request.EstadoProceso, request.Nombres,
                request.ApellidoPaterno, request.ApellidoMaterno, request.Correo
            );

            var excel = new ExcelGenerator();
            excel.GenerateExcel("Reporte de casos", data);

            return File(excel.ToStream(), ExcelGenerator.MimeType, $"{Helpers.ExcelFileName(Helpers.TypeName.DATE, "reporte_casos", DateTime.Now)}");
        }

        [HttpPost("periodo")]
        public ResponseJSON Periodo([FromBody] ReporteCasoRequest request)
        {
            try
            {
                var result = _reporteRepository.ChartTotalesPorPeriodo(request.TipoTramite);
                return new ResponseJSON() { Success = true, Data = result, };
            }
            catch (Exception ex)
            {
                return new ResponseJSON() { Success = false, Message = ex.Message, };
            }
        }

        [HttpGet("periodo/exportar")]
        [AllowAnonymous]
        public ResponseJSON PeriodoExportar([FromQuery] ReporteCasoRequest request)
        {
            return new ResponseJSON()
            {
                Success = false,
            };
        }

        [HttpPost("tiempos")]
        public DataTableJS<ReporteTramiteCasoTiempo> Tiempos([FromForm] DataTableJSRequest<ReporteCasoRequest> dataTable)
        {
            var response = new DataTableJS<ReporteTramiteCasoTiempo>();
            try
            {
                var request = dataTable.AdditionalValues;
                DateTime? fechaInicio = null;
                DateTime? fechaTermino = null;

                if (string.IsNullOrWhiteSpace(request.FechaInicio)) {
                    fechaInicio = Convert.ToDateTime(request.FechaInicio);
                }

                if (string.IsNullOrWhiteSpace(request.FechaTermino)) {
                    fechaTermino = Convert.ToDateTime(request.FechaTermino);
                }

                var data = _reporteRepository.ReporteTiempo(request.TipoReporte ?? 1, request.TipoTramite, request.Mes, request.Anio, fechaInicio, fechaTermino,
                     request.EstadoTipo, request.EstadoProceso, request.Nombres, request.ApellidoPaterno, request.ApellidoMaterno, request.Correo, request.NroDias);

                response.Data = data;
                response.RecordsTotal = data.Count();
                response.RecordsFiltered = data.Count();
                response.Draw = dataTable.Draw;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }

        [HttpGet("tiempos/exportar")]
        [AllowAnonymous]
        public FileStreamResult TiemposExportar([FromQuery] ReporteCasoRequest request)
        {
            DateTime? fechaInicio = null;
            DateTime? fechaTermino = null;

            if (string.IsNullOrWhiteSpace(request.FechaInicio))
                fechaInicio = Convert.ToDateTime(request.FechaInicio);

            if (string.IsNullOrWhiteSpace(request.FechaTermino))
                fechaTermino = Convert.ToDateTime(request.FechaTermino);

            var data = _reporteRepository.ReporteTiempo(request.TipoReporte ?? 1, request.TipoTramite, request.Mes, request.Anio, fechaInicio, fechaTermino,
                     request.EstadoTipo, request.EstadoProceso, request.Nombres, request.ApellidoPaterno, request.ApellidoMaterno, request.Correo, request.NroDias);

            var excel = new ExcelGenerator();
            excel.GenerateExcel("Reporte de tiempo", data);

            string name = string.Empty;

            if (request.TipoReporte == 0)
                name = Helpers.ExcelFileName(Helpers.TypeName.DATE, "Reporte_de_Ventas", DateTime.Now);
            else if (request.TipoReporte == 1)
                name = Helpers.ExcelFileName(Helpers.TypeName.MONTH, "Reporte_de_Ventas", request.Mes ?? 1, request.Anio ?? 2024);
            else if (request.TipoReporte == 2)
                name = Helpers.ExcelFileName(Helpers.TypeName.RANGE, "Reporte_de_Ventas", DateTime.Now.AddDays(-15), DateTime.Now);
            else if (request.TipoReporte == 2)
                name = Helpers.ExcelFileName(Helpers.TypeName.RANGE, "Reporte_de_Ventas", DateTime.Now.AddDays(-7), DateTime.Now);
            else
                name = Helpers.ExcelFileName(Helpers.TypeName.RANGE, "Reporte_de_Ventas", fechaInicio.Value, fechaTermino.Value);

            return File(excel.ToStream(), ExcelGenerator.MimeType, name);
        }
    }
}
