using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RenovLicController : ControllerBase
    {
        private readonly IRenovLicRepository _renovLicRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RenovLicController(IRenovLicRepository renovLicRepository, IEmailRepository er, IClienteRepository cR, IHttpContextAccessor httpContextAccessor)
        {
            _renovLicRepository = renovLicRepository;
            _emailRepository = er;
            _clienteRepository = cR;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseEntity<RenovLic>>> GetRenovLic()
        {
            ResponseEntity<RenovLic> response = new ResponseEntity<RenovLic>();
            response = await _renovLicRepository.GetAllRenovLic();
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseEntity<RenovLic>>> GetRenovLice(int id)
        {
            ResponseEntity<RenovLic> response = new ResponseEntity<RenovLic>();
            response = await _renovLicRepository.GetRenovLicById(id);
            return response;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<RenovLic>>> PostRenovLic(RenovLic renovLic)
        {
            ResponseEntity<RenovLic> response = new ResponseEntity<RenovLic>();
            response = await _renovLicRepository.AddRenovLic(renovLic);
            string nombreForm = "Renovación de Licencia";
            if (response.success)
            {                
                ResponseEntity<Cliente> cliente = await _clienteRepository.GetClienteById(renovLic.cl_cliente.cl_id);
                _emailRepository.EmailConfirmacionFormulario(cliente.item, nombreForm);
            }
            return response;
        }

        [HttpPut]
        public async Task<ActionResult<ResponseEntity<RenovLic>>> PutRenovLi(RenovLic renovLic)
        {
            ResponseEntity<RenovLic> response = new ResponseEntity<RenovLic>();
            response = await _renovLicRepository.UpdateRenovLic(renovLic);
            return response;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResponseEntity<RenovLic>>> DeleteRenovLic(int id)
        {
            ResponseEntity<RenovLic> response = new ResponseEntity<RenovLic>();
            response = await _renovLicRepository.DeleteRenovLic(id);
            return response;
        }

        [HttpGet("{cl_id}/{pg_id}/{tr_id}")]
        public async Task<ActionResult<ResponseEntity<RenovLic>>> GetFrmValidacion(int cl_id, int pg_id, int tr_id)
        {
            ResponseEntity<RenovLic> response = new ResponseEntity<RenovLic>();
            response = await _renovLicRepository.GetRenovLicValidacion(cl_id, pg_id, tr_id);
            return response;
        }


        [HttpGet("FormEstado0")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetTodosLosFormulario0()
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.GetFormEstado0();
            return response;
        }

        [HttpGet("FormEstado1")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetTodosLosFormulario1()
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.GetFormEstado1();
            return response;
        }
        [HttpGet("FormEstado2")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetTodosLosFormulario2()
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.GetFormEstado2();
            return response;
        }
        [HttpGet("FormEstado3")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetTodosLosFormulario3()
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.GetFormEstado3();
            return response;
        }

        [HttpGet("obtenerDatosForm/{cl_id}/{tr_id}/{frm_id}")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetDatosCompletosClienteForm(int cl_id, int tr_id, int frm_id)
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.GetDatosCompletoForm(cl_id, tr_id, frm_id);
            return response;
        }


        [HttpPut("actualizarDatosForm")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> UpdateDatosCompletoFormPanelCliente(FormularioDTO formularioDTO)
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.UpdateDatosCompletoFormPanel(formularioDTO);
            return response;
        }


        [HttpPut("cambioEstadoForm/{tr_id}/{frm_id}/{frm_estado}")]
        public async Task<ActionResult<ResponseEntity<int>>> PutcambioEstadoForm(int tr_id, int frm_id, int frm_estado)
        {
            ResponseEntity<int> response = new ResponseEntity<int>();
            response = await _renovLicRepository.cambioEstadoForm(tr_id, frm_id, frm_estado);
            return response;
        }

     
        //[HttpPut("cambioEstadoProcessForm/{tr_id}/{frm_id}/{frm_estado}")]
        //public async Task<ActionResult<ResponseEntity<int>>> PutcambioEstadoProcessForm(int tr_id, int frm_id, int frm_estado)
        //{
        //    ResponseEntity<int> response = new ResponseEntity<int>();
        //    response = await _renovLicRepository.cambioEstadoProcesoForm(tr_id, frm_id, frm_estado);
        //    return response;
        //}

        [HttpPut("cambioEstadoProcessForm/{tr_id}/{frm_id}/{frm_estado}")]
        public async Task<ActionResult<ResponseEntity<int>>> PutcambioEstadoProcessForm(int tr_id, int frm_id, int frm_estado, [FromBody] CambioEstadoRequest request)
        {
            ResponseEntity<int> response = new ResponseEntity<int>();
            response = await _renovLicRepository.cambioEstadoProcesoForm(tr_id, frm_id, frm_estado, request.motivo);
            return response;
        }


        [HttpGet("obtenerRegistro")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetRegistro1()
        {
            ResponseEntity<FormularioDTO> response = new ResponseEntity<FormularioDTO>();
            response = await _renovLicRepository.GetObtenerRegistro1();
            return response;
        }


        [HttpPost("buscadorRegistroPanel")]
        public async Task<ActionResult<ResponseEntity<FormularioDTO>>> GetBuscadorRegistroPanel(string item)
        {
            var context = _httpContextAccessor.HttpContext;
            var form = context.Request.Form;
            var item2 = form["item"];
            item = item == null ? item2 : item;
            PaginatorEntity paginator = JsonConvert.DeserializeObject<PaginatorEntity>(form["paginator"]);
            FormularioDTO archivo = JsonConvert.DeserializeObject<FormularioDTO>(item);
            ResponseEntity<FormularioDTO> response = await _renovLicRepository.GetBuscarReportePanel(archivo, paginator);
            return response;
        }



        [HttpPost("buscadorRegistroPanelExcel")]
        public async Task<IActionResult> GetBuscarReportePanel2(string item)
        {
            var context = _httpContextAccessor.HttpContext;
            var form = context.Request.Form;
            var item2 = form["item"];
            item = item == null ? item2 : item;

            FormularioDTO archivo = JsonConvert.DeserializeObject<FormularioDTO>(item);
            ResponseEntity<FormularioDTO> response = await _renovLicRepository.GetBuscarReportePanelExcel(archivo);

            if (response == null || response.items == null)
            {
                return NotFound("No data found");
            }

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Datos");

                // Añade los encabezados de las columnas
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Nombre Completo";
                worksheet.Cell(1, 3).Value = "Nombre";
                worksheet.Cell(1, 4).Value = "Primer Apellido";
                worksheet.Cell(1, 5).Value = "Segundo Apellido";
                worksheet.Cell(1, 6).Value = "Correo";
                worksheet.Cell(1, 7).Value = "Teléfono";
                worksheet.Cell(1, 8).Value = "Código Pago";
                worksheet.Cell(1, 9).Value = "Fecha Pago";
                worksheet.Cell(1, 10).Value = "Estado";
                worksheet.Cell(1, 11).Value = "Estado Proceso";
                worksheet.Cell(1, 12).Value = "Nombre Trámite";
                worksheet.Cell(1, 13).Value = "ID Trámite";

                // Rellena las filas con los datos de response.Data
                int row = 2;
                foreach (var itemData in response.items)
                {
                    worksheet.Cell(row, 1).Value = itemData.FrmID;
                    worksheet.Cell(row, 2).Value = itemData.cl_cliente.cl_nombreCompleto;
                    worksheet.Cell(row, 3).Value = itemData.cl_cliente.cl_nombre;
                    worksheet.Cell(row, 4).Value = itemData.cl_cliente.cl_primerApellido;
                    worksheet.Cell(row, 5).Value = itemData.cl_cliente.cl_segundoApellido;
                    worksheet.Cell(row, 6).Value = itemData.cl_cliente.cl_correo;
                    worksheet.Cell(row, 7).Value = itemData.cl_cliente.cl_numeroTelefono;
                    worksheet.Cell(row, 8).Value = itemData.cl_pago.pg_codigo;
                    worksheet.Cell(row, 9).Value = itemData.cl_pago.pg_fecha != DateTime.MinValue ? itemData.cl_pago.pg_fecha.ToString("yyyy-MM-dd") : "";
                    worksheet.Cell(row, 10).Value = itemData.Estado;
                    worksheet.Cell(row, 11).Value = itemData.EstadoProceso;
                    worksheet.Cell(row, 12).Value = itemData.NombreTramite;
                    worksheet.Cell(row, 13).Value = itemData.cl_tramite.tr_id;
                    row++;
                };

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReportePanel.xlsx");
                }
            }
        }

    }

}
