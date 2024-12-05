using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicencia.Utility;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReporteVentaController : ControllerBase
    {
        private readonly ReporteCasoRepository _reporteCasoRepository;

        public ReporteVentaController(IConfiguration configuration)
        {
            _reporteCasoRepository = new ReporteCasoRepository(configuration);
        }

        [HttpPost("general")]
        public ResponseJSON Index(ReporteCasoRequest request)
        {
            var response = new ResponseJSON();
            try
            {
                DateTime? fechaInicio = null;
                DateTime? fechaTermino = null;

                if (!string.IsNullOrWhiteSpace(request.FechaInicio)) {
                    fechaInicio = Convert.ToDateTime(request.FechaInicio);
                }

                if (!string.IsNullOrWhiteSpace(request.FechaTermino)) {
                    fechaTermino = Convert.ToDateTime(request.FechaTermino);
                }

                response.Data = _reporteCasoRepository.ReporteVenta(request.TipoReporte, request.TipoTramite, request.Mes, request.Anio, fechaInicio, fechaTermino);
                response.Success = true;
                response.Message = "";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        [HttpPost("detalle")]
        public DataTableJS<ReporteDetalleVenta> Detalle([FromForm] DataTableJSRequest<ReporteCasoRequest> dataTable)
        {
            var response = new DataTableJS<ReporteDetalleVenta>();
            try
            {
                var request = dataTable.AdditionalValues;
                DateTime? fechaInicio = null;
                DateTime? fechaTermino = null;

                if (!string.IsNullOrWhiteSpace(request.FechaInicio)) {
                    fechaInicio = Convert.ToDateTime(request.FechaInicio);
                }

                if (!string.IsNullOrWhiteSpace(request.FechaTermino)) {
                    fechaTermino = Convert.ToDateTime(request.FechaTermino);
                }

                var data = _reporteCasoRepository.ReporteDetalleVenta(request.TipoReporte, request.TipoTramite, request.Mes, request.Anio, fechaInicio, fechaTermino);
                var total = data.Count();
                response.Data = data;
                response.Draw = dataTable.Draw;
                response.RecordsFiltered = total;
                response.RecordsTotal = total;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }


        [HttpPost("multa")]
        public DataTableJS<ReporteMulta> Multa([FromForm] DataTableJSRequest<ReporteCasoRequest> dataTable)
        {
            var response = new DataTableJS<ReporteMulta>();
            try
            {
                var request = dataTable.AdditionalValues;
                var data = _reporteCasoRepository.ReporteMulta(request.cl_nombre, request.cl_correo, request.cl_numeroTelefono, request.pg_codigo, request.tr_id, request.fecha,request.estado);
                var total = data.Count();
                response.Data = data;
                response.Draw = dataTable.Draw;
                response.RecordsFiltered = total;
                response.RecordsTotal = total;
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }

        [HttpGet("exportar")]
        [AllowAnonymous]
        public FileStreamResult Exportar([FromQuery] ReporteCasoRequest request)
        {
            DateTime? fechaInicio = null;
            DateTime? fechaTermino = null;

            if (!string.IsNullOrWhiteSpace(request.FechaInicio))
                fechaInicio = Convert.ToDateTime(request.FechaInicio);

            if (!string.IsNullOrWhiteSpace(request.FechaTermino))
                fechaTermino = Convert.ToDateTime(request.FechaTermino);

            var data = _reporteCasoRepository.ReporteDetalleVenta(request.TipoReporte, request.TipoTramite, request.Mes, request.Anio, fechaInicio, fechaTermino);
            var excel = new ExcelGenerator();
            excel.GenerateExcel("Reporte de venta", data);

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
