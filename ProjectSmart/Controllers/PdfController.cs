using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using SmartLicense.Pdf;
using SmartLicencia.Entity;
using SmartLicense.Pdf.Models;
using System.Xml.Linq;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : BuilderPDFController
    {
        private readonly CertificadoRepository _certificadoRepository;
        private string _rootDirectory;

        public PdfController(IWebHostEnvironment env, IConfiguration config)
            : base(env, config)
        {
            _certificadoRepository = new CertificadoRepository(config);
            _rootDirectory = Path.Combine(env.WebRootPath, $"Docs");
        }

        [HttpGet("certificado/{trId}/{frmId}")]
        [HttpGet("gen-certificado/{trId}/{frmId}/{regenerate}")]
        public async Task<IActionResult> Certificado(int trId, int frmId, int regenerate)
        {
            try
            {
                string template = "Certificado";

                if (trId == 4)
                    template = "CertificadoReciprocidad";

                var fileName = Path.Combine(_rootDirectory, $"{trId}-{frmId}.pdf");

                if (regenerate == 0 && !System.IO.File.Exists(fileName))
                    regenerate = 1;

                if (regenerate == 1)
                {
                    var model = _certificadoRepository.ObtenerCertificado(trId, frmId);
                    model.Titulo = fileName;

                    await GeneratePdfOfTemplate(template, fileName, model);
                }

                return renderPdf(fileName);
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/FileError.cshtml", new FileNotFoundException("Error al intentar generar certificado.", ex));
            }
        }

        [HttpGet("consolidado/{trId}/{frmId}")]
        [HttpGet("gen-consolidado/{trId}/{frmId}/{regenerate}")]
        public async Task<IActionResult> Consolidado(int trId, int frmId, int regenerate)
        {
            try
            {
                var fileName = Path.Combine(_rootDirectory, $"EXP-CASO-{trId}-{frmId}.pdf");
                List<string> combinesFiles = new List<string>();

                var pgId = _certificadoRepository.Obtener_pgId(trId, frmId);
                var originalFiles = _certificadoRepository.ObtenerArchivos(trId, pgId);
                var files = originalFiles.Where(x => !x.Nombre.Contains("Firma") && !(x.Url ?? "").EndsWith(".pdf"));
                var imagenes = new ImagePdf(files);
                var imagenenfirma = _certificadoRepository.ObtenerFirma(trId, pgId).Imagenes.Where(x => x.Key == "Firma").FirstOrDefault();
                var imagenenfirmadoctor = _certificadoRepository.ObtenerFirmaDoctor(trId, frmId).Imagenes.Where(x => x.Key == "FirmaDoctor").FirstOrDefault();

                if (regenerate == 0 && !System.IO.File.Exists(fileName))
                    regenerate = 1;

                if (regenerate == 1)
                {
                    string tramite = string.Empty;
                    var nombresArchivos = imagenes.Imagenes.Keys.ToList();
                    
                    var tramiteRepo = HttpContext.RequestServices.GetService<ITramiteRepository>();

                    if (tramiteRepo != null)
                    {
                        var tramiteFind = tramiteRepo.ListarTramites().Where(x => x.tr_id == trId).FirstOrDefault();

                        if (tramiteFind != null)
                            tramite = tramiteFind.tr_nombre.ToUpper();
                    }

                    var pdfPathIndice = PDF.ResolveTempFileName($"idx-{trId}-{frmId}");

                    if(Array.Exists(new int[] { 1, 2, 3, 4 }, x => x == trId))
                    {
                        var certificadoInfo = _certificadoRepository.ObtenerCertificado(trId, frmId);
                        var certificadoMedico = _certificadoRepository.CertificadoMedico(trId, frmId);

                        if (trId != 4 && certificadoMedico != null)
                            nombresArchivos.Add("Certificado Médico");

                        await GeneratePdfOfTemplate("Indice", pdfPathIndice, new IndexPdf
                        {
                            Caso = $"{certificadoInfo.PrimerNombre} {certificadoInfo.SegundoNombre} {certificadoInfo.ApellidoPaterno} {certificadoInfo.ApellidoMaterno}",
                            Formulario = trId == 4 ? "DTOP-DIS-257" : "DTOP-DIS-256",
                            Titulo = certificadoInfo.Titulo,
                            Fecha = certificadoInfo.FechaReg,
                            Tramite = tramite,
                            Archivos = nombresArchivos,
                        });

                        combinesFiles.Add(pdfPathIndice);

                        if (imagenenfirma.Key != null)
                            certificadoInfo.Firma = imagenenfirma.Value;

                        var pathPdfCertificado = Path.Combine(_rootDirectory, $"{trId}-{frmId}.pdf");

                        if (!System.IO.File.Exists(pathPdfCertificado) || regenerate == 1)
                            await GeneratePdfOfTemplate((trId == 4 ? "CertificadoReciprocidad" : "Certificado"), pathPdfCertificado, certificadoInfo);

                        combinesFiles.Add(pathPdfCertificado);

                        if (imagenes != null)
                        {
                            foreach (var item in imagenes.Imagenes)
                            {
                                var pdfImg = await PDF.ImageToPdf(item.Value, item.Key);

                                if(!string.IsNullOrWhiteSpace(pdfImg))
                                    combinesFiles.Add(pdfImg);
                            }
                        }                        

                        var pathCertificadoAutorizacion = _certificadoRepository.GetAutorizacion(trId, frmId, pgId, _rootDirectory);

                        combinesFiles.Add(pathCertificadoAutorizacion);

                        if (trId != 4)
                        {
                            var pathPdfCertificadoMedico = Path.Combine(_hostEnv.WebRootPath, $"Evaluaciones", "evaluacion_" + frmId + "_" + trId + ".pdf");

                            if (certificadoMedico != null)
                            {
                                if (imagenenfirma.Key != null)
                                    certificadoMedico.Firma = imagenenfirma.Value;

                                if (imagenenfirmadoctor.Key != null)
                                    certificadoMedico.FirmaDoctor = $"https://api.tulicenciapr.com/{imagenenfirmadoctor.Value}?t={DateTime.Now.Ticks}";

                                if (!System.IO.File.Exists(pathPdfCertificadoMedico) || regenerate == 1)
                                    await GeneratePdfOfTemplate("CertificadoMedico", pathPdfCertificadoMedico, certificadoMedico);

                                combinesFiles.Add(pathPdfCertificadoMedico);
                            }
                        }
                    } else
                    {
                        var pathPdfImagenes = Path.Combine(_rootDirectory, $"IMG-{trId}-{frmId}.pdf");
                        var attachmentFiles = new List<ItemImagen>();

                        if (trId == 5)
                        {
                            nombresArchivos = new List<string>();
                            attachmentFiles = originalFiles.Where(x => x.Position <= 6)
                                .GroupBy(x => x.Position)
                                .Select(x => x.First()).ToList();

                            foreach (var item in attachmentFiles)
                                nombresArchivos.Add(item.Nombre);  
                        }

                        await GeneratePdfOfTemplate("Indice", pdfPathIndice, new IndexPdf
                        {
                            Caso = string.Empty,
                            Formulario = string.Empty,
                            Titulo = string.Empty,
                            Fecha = string.Format("{0:MM/dd/yyyy HH:mm:ss tt}", DateTime.Now),
                            Tramite = tramite,
                            Archivos = nombresArchivos,
                        });

                        combinesFiles.Add(pdfPathIndice);

                        foreach (var item in attachmentFiles)
                        {
                            if(!string.IsNullOrWhiteSpace(item.Url))
                            {
                                var filePathTemp = await PDF.ImageToPdf(item.Url, item.Nombre);

                                combinesFiles.Add(filePathTemp);
                            }
                        }
                    }

                    PDF.CombinePdfs(fileName, combinesFiles.ToArray());

                    await Task.Delay(1000);
                }

                return renderPdf(fileName);
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/FileError.cshtml", new FileNotFoundException("Error al intentar generar expediente del caso.", ex));
            }
        }


        [HttpGet("gen-certificado-medico/{trId}/{frmId}/{regenerate}")]
        public async Task<IActionResult> CertificadoMedico(int trId, int frmId, int regenerate)
        {
            try
            {
                string fileName = Path.Combine(_hostEnv.WebRootPath, $"Evaluaciones", "evaluacion_" + frmId + "_" + trId + ".pdf");

                if (!System.IO.File.Exists(fileName))
                    regenerate = 1;

                if (regenerate == 1)
                {
                    var pgId = _certificadoRepository.Obtener_pgId(trId, frmId);
                    var firma = _certificadoRepository.ObtenerFirma(trId, pgId).Imagenes.Where(x => x.Key == "Firma").FirstOrDefault();
                    var firmaDoctor = _certificadoRepository.ObtenerFirmaDoctor(trId, frmId).Imagenes.Where(x => x.Key == "FirmaDoctor").FirstOrDefault();
                    var certificadoMedico = _certificadoRepository.CertificadoMedico(trId, frmId);

                    if (firma.Key != null)
                        certificadoMedico.Firma = firma.Value;

                    if (firmaDoctor.Key != null)
                        certificadoMedico.FirmaDoctor = $"https://api.tulicenciapr.com/{firmaDoctor.Value}?t={DateTime.Now.Ticks}";

                    if (!System.IO.File.Exists(fileName) || regenerate == 1)
                        await GeneratePdfOfTemplate("CertificadoMedico", fileName, certificadoMedico);
                }

                return renderPdf(fileName);
            }
            catch (Exception ex)
            {
                return View("~/Views/Errors/FileError.cshtml", new FileNotFoundException("Error al intentar generar certificado médico.", ex));
            }
        }

        [HttpPost("sign")]
        public async Task<ResponseJSON> Sign([FromBody] SignPDFRequest signRequest)
        {
            var response = new ResponseJSON();
            try
            {
                string fileName = Path.Combine(_rootDirectory, "Sign", $"{signRequest.FileName}.pdf");
                string fileOutput = Path.Combine(_rootDirectory, "Sign", $"{signRequest.FileName}_signed.pdf");
                string signPath = Path.Combine(_rootDirectory, "Sign", $"{signRequest.FileName}_{signRequest.SignName}.png");

                await PDF.DownloadFileAsync(signRequest.Url, fileName, true);
                await PDF.SaveBase64AsPng(signRequest.Sign, signPath);
                await PDF.Signature(fileName, fileOutput, signPath, signRequest.X, signRequest.Y, signRequest.Page, signRequest.Width, signRequest.Height);

                response.Success = true;
                response.Message = "Documento firmado correctamente.";
                response.Data = new {
                    original = fileName,
                    signed = fileOutput,
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.Data = ex;
            }
            return response;
        }        
    }
}
