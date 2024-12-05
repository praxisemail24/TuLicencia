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
    public class MultaController : Controller
    {
        private readonly MultaRepository _multaRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IWebHostEnvironment _webEnvironment;
        private readonly SenderMail _sender;

        public MultaController(IConfiguration config, IWebHostEnvironment env, IClienteRepository cliente)
        {
            _multaRepository = new MultaRepository(config);
            _webEnvironment = env;
            _clienteRepository = cliente;
            _sender = new SenderMail(new PlantillaMensajeRepository(config), config);
        }

        [HttpGet("Info/{tr_id}/{frm_id}/{cl_id}")]
        public async Task<MultaResponse> GetInfo(int tr_id, int frm_id, int cl_id)
        {
            var response = new MultaResponse();
            try
            {
                if (tr_id == 0 && frm_id == 0 && cl_id == 0)
                    throw new Exception("Se requiere parámetros de cliente.");

                var cliente = await _clienteRepository.GetClienteById(cl_id);
                response.NroLicencia = cliente.item.cl_numeroLicencia;
                response.Correo = cliente.item.cl_correo;
                response.NombreCliente = $"{cliente.item.cl_nombre} {cliente.item.cl_primerApellido} {cliente.item.cl_segundoApellido}";
                response.NroSSN = cliente.item.cl_numeroSeguro;
                response.FechaNacValue = cliente.item.cl_fechaNacimiento;
                response.Multas = _multaRepository.Listar(tr_id, frm_id);
                response.Archivos = _multaRepository.ListarArchivos(tr_id, frm_id);
                response.PagoCesco= _multaRepository.ObtenerPagoCesco(tr_id, frm_id,cl_id);
                response.PagoAutoExpress = _multaRepository.ObtenerPagoAutoExpress(tr_id, frm_id, cl_id);
                response.TipoPago= _multaRepository.ObtenerPagoCescoTipo(tr_id, frm_id, cl_id);

                var pago = _multaRepository.ObtenerPago(tr_id, frm_id);
                if( pago != null )
                {
                    pago.Cuotas = _multaRepository.ListarCuotas(pago.Id).ToList();
                    response.Pago = pago;
                }
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
                Logger.Error(ex);
            }
            return response;
        }

        [HttpGet("traeDatoPago/{codigoPago}")]
        public async Task<DetallePago> traeDatoPago(string codigoPago)
        {
            var response = new DetallePago();
            try
            {
                var pagoDetalle2 = await _clienteRepository.GetDetallePago(codigoPago);


                var cliente = pagoDetalle2.items.First();

                response.nombre = cliente.nombre;
                response.codigoPago = cliente.codigoPago;
                response.fecha = cliente.fecha;
                response.tramite = cliente.tramite;
                response.tramitetabla = cliente.tramitetabla;
                response.monto = cliente.monto;
                response.subtotal = cliente.subtotal;
                response.total = cliente.total;
                response.telefono = cliente.telefono;
                response.direccion = cliente.direccion;
                response.transaccion = cliente.transaccion;
            }
            catch (Exception ex)
            {
                //response.Error = ex.Message;
            }
            return response;
        }




        


        [HttpGet("Listar/{tr_id}/{frm_id}")]
        public IEnumerable<Multa> Listar(int tr_id, int frm_id)
        {
            return _multaRepository.Listar(tr_id, frm_id);
        }

        [HttpPost("Store")]
        public ResponseJSON Store([FromBody] Multa multa)
        {
            var response = new ResponseJSON();
            try
            {
                var st = _multaRepository.GuardarMulta(multa);

                response.Success = st;
                response.Message = st ? "Se ha guardado correctamente el registro." : "Error al intentar guardar registro.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("StoreBorra")]
        public ResponseJSON StoreBorra([FromBody] Multa multa)
        {
            var response = new ResponseJSON();
            try
            {
                var st = _multaRepository.BorraMulta(multa);

                response.Success = st;
                response.Message = st ? "Se ha borrado correctamente el registro." : "Error al intentar borrar registro.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("PlanPago")]
        public ResponseJSON GuardarPago([FromBody] MultaPago pago)
        {
            var response = new ResponseJSON();
            try
            {
                if (pago == null)
                    throw new Exception("Se requiere información de pago.");

                var id = _multaRepository.GuardarPago(pago);
                
                if(id > 0)
                {
                    foreach (var item in pago.Cuotas)
                    {
                        item.MultaPagoId = id;
                        _multaRepository.GuardarCuota(item);
                    }
                }

                response.Success = true;
                response.Message = "Se ha guardado correctamente las cuotas.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpPost("SubirCertificado/{tr_id}/{frm_id}")]
        public async Task<ResponseJSON> SubirCertificado(IFormFile Certificado, [FromForm] int TramiteId, [FromForm] int FormularioId, [FromForm] int AutorId, [FromForm] string Origen)
        {
            var response = new ResponseJSON();
            try
            {
                var directory = Path.Combine(_webEnvironment.WebRootPath, "CertMultas");

                if(!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var fileName = string.Format("{0}-{1}__{2:MMddyyyy}_{3}", TramiteId, FormularioId, DateTime.Now, Certificado.FileName);
                var filePathName = Path.Combine(directory, fileName);

                var stream = new FileStream(filePathName, FileMode.Create);
                await Certificado.CopyToAsync(stream);
                stream.Close();

                _multaRepository.GuardarArchivo(new MultaArchivo
                {
                    AutorId = AutorId,
                    FormularioId = FormularioId,
                    TramiteId = TramiteId,
                    MimeType = Certificado.ContentType,
                    NombreArchivo = fileName,
                    RootPath = filePathName,
                    Url = $"https://api.tulicenciapr.com/api/Multa/Descargar?file={fileName}",
                    Tamanio = Certificado.Length,
                    Origen = Origen,
                });

                response.Success = true;
                response.Message = "Archivo de certificado subido correctamente.";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }

        [HttpGet("Descargar")]
        [AllowAnonymous]
        public FileResult Descargar([FromQuery] string file)
        {
            var path = Path.Combine(_webEnvironment.WebRootPath, "CertMultas", file);
            var ext = Path.GetExtension(file);
            
            string mimeType = "application/octet-stream";

            switch (mimeType)
            {
                case ".png":
                    mimeType = "image/png";
                    break;
                case ".jpg":
                    mimeType = "image/jpg";
                    break;
                case ".jpeg":
                    mimeType = "image/jpeg";
                    break;
                case ".pdf":
                    mimeType = "application/pdf";
                    break;
                case ".svg":
                    mimeType = "image/svg+xml";
                    break;
                case ".docx":
                    mimeType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xlsx":
                    mimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".zip":
                    mimeType = "application/zip";
                    break;
                default:
                    break;
            }

            var fs = new FileStream(path, FileMode.Open);
            return File(fs, mimeType, file);
        }

        [HttpPost("EnviarCorreo")]
        public async Task<ResponseJSON> EnviarCorreo([FromBody] EmailPagoRequest request)
        {
            var response = new ResponseJSON();
            try
            {
                string templateName = string.Empty;
                string tipoTramite = string.Empty;
                List<KeyValuePair<string, object>> parameters = new List<KeyValuePair<string, object>>();
                var mail = new MailPartialBody();

                mail.Variables.Add("cliente_id", request.ClienteId);
                mail.Variables.Add("tramite_id", request.TramiteId);
                mail.Variables.Add("formulario_id", request.FormularioId);
                mail.Variables.Add("origen_multas", request.Origen);

                switch (request.TramiteId)
                {
                    case 1:
                        tipoTramite = "RENOVACIÓN DE LICENCIA";
                        break;
                    case 2:
                        tipoTramite = "LICENCIA DE APRENDIZAJE";
                        break;
                    case 3:
                        tipoTramite = "DUPLICADO DE LICENCIA";
                        break;
                    case 4:
                        tipoTramite = "LICENCIA DE RECIPROCIDAD";
                        break;
                    default:
                        break;
                }

                switch (request.Correo)
                {
                    case 0:
                        templateName = Constants.CORREO_MULTAS_PENDIENTES;
                        var multas = _multaRepository.Listar(request.TramiteId, request.FormularioId).Where(x => x.Origen == request.Origen);
                        mail.Variables.Add("lista_multas", HtmlGenerator.GenerateTable(multas.ToList()));
                        mail.Variables.Add("total_multas", multas.Sum(x => x.Monto).ToHtml());
                        break;
                    case 1:
                        templateName = Constants.CORREO_CONFIRMACION_PAGO_TOTAL;
                        break;
                    case 2:
                        templateName = Constants.CORREO_CONFIRMACION_PAGO_CUOTA;
                        break;
                    case 3:
                        templateName = Constants.CORREO_TRAMITE_DETENIDO;
                        var multas2 = _multaRepository.Listar(request.TramiteId, request.FormularioId).Where(x => x.Origen == request.Origen);
                        mail.Variables.Add("lista_multas", HtmlGenerator.GenerateTable(multas2.ToList()));
                        mail.Variables.Add("total_multas", multas2.Sum(x => x.Monto).ToHtml());
                        break;
                    default:
                        break;
                }

                if (string.IsNullOrWhiteSpace(templateName))
                    throw new Exception("Tipo de correo inválido");

                var cliente = await _clienteRepository.GetClienteById(request.ClienteId);

                if (!_sender.IsValidEmail(cliente.item.cl_correo))
                    throw new Exception("Verifique que el correo electrónico del cliente sea válido.");

                string fullName = $"{cliente.item.cl_nombre} {cliente.item.cl_primerApellido} {cliente.item.cl_segundoApellido}";

                mail.To.Add(new SenderMailAddress(cliente.item.cl_correo, fullName));
                mail.Variables.Add("tipo_tramite", tipoTramite);
                mail.Variables.Add("cliente_nombre", fullName);
                mail.Variables.Add("cliente_correo", cliente.item.cl_correo);
                mail.Variables.Add("nro_licencia", cliente.item.cl_numeroLicencia);
                mail.Variables.Add("nro_seguro", cliente.item.cl_numeroSeguro);

                var status = _sender.Send(templateName, mail);

                response.Success = true;
                response.Message = "Correo enviado correctamente.";
                response.Data = status;
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
