using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicense.Pdf.Models;
using SmartLicense.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Controlador de Record Choferil")]
    public class RecordChoferilController : BuilderPDFController
    {
        private readonly RecordChoferilRepository _recordChoferilRepository;
        private readonly IArchivoRepository _archivoRepository;
        private readonly Tramite _tramite;

        public RecordChoferilController(IWebHostEnvironment webHost, IConfiguration config, IArchivoRepository archivoRepository, ITramiteRepository tramiteRepository)
            :base(webHost, config)
        { 
            _archivoRepository = archivoRepository;
            _recordChoferilRepository = new RecordChoferilRepository(config);
            _tramite = tramiteRepository.TramiteById(10);
        }

        private async Task<ResponseJSON> _store(RecordChoferilRequest request, Action? callback = null)
        {
            var response = new ResponseJSON();
            try
            {
                if (request == null)
                    throw new Exception("Se requiere parámetros de registro.");

                if (request.ClId == 0)
                    throw new Exception("Se requiere información de cliente.");

                if (request.PgId == 0)
                    throw new Exception("No se ha especificado un pago para este trámite.");

                if (string.IsNullOrWhiteSpace(request.LicensePlate))
                    throw new Exception("Se requiere número de tablilla (Placa) del vehículo.");

                if (string.IsNullOrWhiteSpace(request.PurposeApplication))
                    throw new Exception("Se requiere propósito de la solicitud.");

                callback?.Invoke();

                var model = new RecordChoferil
                {
                    Id = request.Id,
                    Lang = request.Lang,
                    CertifiedType = request.Type,
                    Category = request.Category,
                    LicensePlate = request.LicensePlate,
                    PgId = request.PgId,
                    ClId = request.ClId,
                    Serie = request.Serie,
                    Number = request.Number,
                    ExpeditionAt = request.ExpeditionAt,
                    ExpirationAt = request.ExpirationAt,
                    PurposeApplication = request.PurposeApplication,
                };

                model = _recordChoferilRepository.Store(model);

                if(!string.IsNullOrWhiteSpace(request.SignatureApplicant))
                {
                    var fileName = await Utils.CreateFile(request.SignatureApplicant, $"firma_{Utils.Slug(_tramite.tr_nombre)}_cl-{request.ClId}", "C:\\inetpub\\wwwroot\\distTulicencia\\upload");

                    var f = _archivoRepository.AddOrUpdate(new Archivo
                    {
                        pg_id = request.PgId,
                        cl_cliente = new Cliente { cl_id = request.ClId },
                        frm_id = Convert.ToInt32(model.Id),
                        ar_nombre = $"https://tulicenciapr.com/upload/{fileName}",
                        ar_pos = 30,
                        tr_tramite = new Tramite { tr_id = _tramite.tr_id, },
                        ar_estado = 1,
                        ar_fecha = DateTime.Now,
                    });

                    if (!f)
                        throw new Exception("Error al intentar guardar firma del cliente.");
                }

                response.Success = true;
                response.Data = model;
            }
            catch (Exception ex)
            {
                response.Error = ex;
                response.Success = false;
            }
            return response;
        }

        [HttpPost("Store")]
        [SwaggerOperation(
            Summary = "Crea un nuevo record choferil",
            Description = "Registra un nuevo record choferil y devuelve si se ejecuto correctamente el proceso junto con el identificador del nuevo registro.",
            OperationId = "store",
            Tags = new string[] { "RecordChoferil" }
        )]
        public async Task<ResponseJSON> Store([FromBody] RecordChoferilRequest request)
        {
            return await _store(request);
        }

        [HttpPut("Update")]
        [SwaggerOperation(
            Summary = "Actualiza un record choferil",
            Description = "Actualiza un record choferil por ID y devuelve si se ejecuto correctamente el proceso.",
            OperationId = "update",
            Tags = new string[] { "RecordChoferil" }
        )]
        public async Task<ResponseJSON> Update([FromBody] RecordChoferilRequest request)
        {
            return await _store(request, () =>
            {
                if (request.Id == 0)
                    throw new Exception("Se requiere identificador de registro.");
            });
        }

        [HttpGet("{id}/Show")]
        [SwaggerOperation(
            Summary = "Muestra información de record choferil",
            Description = "Muestra información de record choferil por ID.",
            OperationId = "show",
            Tags = new string[] { "RecordChoferil" }
        )]
        public ResponseJSONModel<RecordChoferil> Show(long id)
        {
            var response = new ResponseJSONModel<RecordChoferil>();
            try
            {
                var model = _recordChoferilRepository.GetById(id);

                response.Success = true;
                response.Data = model;
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex;
            }
            return response;
        }

        [HttpGet("{id}/Certificate")]
        [HttpGet("{id}/Certificate/{regenerate:int}")]
        [AllowAnonymous]
        [SwaggerOperation(
            Summary = "PDF record choferil",
            Description = "Genera el PDF de record choferil por ID.",
            OperationId = "Certificate",
            Tags = new string[] { "RecordChoferil" }
        )]
        public async Task<IActionResult> Certificate(int id, int regenerate)
        {
            var fileName = Path.Combine(_hostEnv.ContentRootPath, "wwwroot", "RecordChoferil", $"certificate_{id}.pdf");

            if (!System.IO.File.Exists(fileName))
                regenerate = 1;

            if(regenerate == 1)
            {
                var model = _recordChoferilRepository.GetById(id);

                var pdf = new RecordChoferilPdf
                {

                };
                fileName = await GeneratePdfOfTemplate("RecordChoferil", fileName, pdf);
            }
            
            return renderPdf(fileName);
        }
    }
}
