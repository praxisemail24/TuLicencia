using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using System.Data.SqlClient;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ArchivoController : ControllerBase
    {
        private readonly IArchivoRepository _archivoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEmailRepository _emailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArchivoController(IArchivoRepository archivoRepository, IEmailRepository emailRepository, IClienteRepository clienteRepository, IHttpContextAccessor httpContextAccessor)
        {
            _archivoRepository = archivoRepository;
            _emailRepository = emailRepository;
            _clienteRepository = clienteRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<Archivo>>> PostArchivo(Archivo archivo)
        {
            ResponseEntity<Archivo> response = new ResponseEntity<Archivo>();
            response = await _archivoRepository.AddArchivo(archivo);
            return response;
        }

        [HttpGet("{pg_id}")]
        public async Task<ActionResult<ResponseEntity<Archivo>>> GetArchivos(int pg_id)
        //public async Task<ActionResult<ResponseEntity<Archivo>>> GetArchivos(int frm_id)
        {
            ResponseEntity<Archivo> response = await _archivoRepository.GetArchivoByFrmId(pg_id);
            return response;
        }

        [HttpDelete("{ar_id}")]
        public async Task<ActionResult<ResponseEntity<Archivo>>> DeleteArchivo(int ar_id)
        {
            ResponseEntity<Archivo> response = new ResponseEntity<Archivo>();
            response = await _archivoRepository.DeleteArchivoByFrmId(ar_id);
            return response;
        }


        [HttpPost("Upload2")]
        public async Task<ActionResult<ResponseEntity<Archivo>>> PostArchivo2(IFormFile file, [FromForm] string item)
        {
            ResponseEntity<Archivo> response = new ResponseEntity<Archivo>();
            var context = _httpContextAccessor.HttpContext;

            Archivo archivo = JsonConvert.DeserializeObject<Archivo>(item);
            string rutaGuardar = Path.Combine("", "C:\\inetpub\\wwwroot\\distTulicencia\\upload");

            if (!Directory.Exists(rutaGuardar))
            {
                Directory.CreateDirectory(rutaGuardar);
            }

            try
            {
                string nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}_{file.FileName}";
                string rutaArchivo = Path.Combine(rutaGuardar, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                archivo.ar_nombre = "https://tulicenciapr.com/upload/" + nombreArchivo;
                response = await _archivoRepository.AddArchivo(archivo);
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }

        [HttpPost("Upload")]
        public async Task<ActionResult<ResponseEntity<Archivo>>> PostArchivo(IFormFile file, String archivo2)
        {
            ResponseEntity<Archivo> response = new ResponseEntity<Archivo>();
            var context = _httpContextAccessor.HttpContext;
            var form = context.Request.Form;
            var item = form["item"];
            file = file == null ? form.Files.FirstOrDefault() : file;
            Archivo archivo = JsonConvert.DeserializeObject<Archivo>(item);
            string rutaGuardar = Path.Combine("", "C:\\inetpub\\wwwroot\\distTulicencia\\upload");

            if (!Directory.Exists(rutaGuardar))
            {
                Directory.CreateDirectory(rutaGuardar);
            }

            try
            {
                string nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}_{file.FileName}";                
                string rutaArchivo = Path.Combine(rutaGuardar, nombreArchivo);
                // Copiamos el contenido del archivo al nuevo archivo en el servidor
                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                archivo.ar_nombre = "https://tulicenciapr.com/upload/" + nombreArchivo;
                response = await _archivoRepository.AddArchivo(archivo);
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }            
            return response;
        }


        [HttpPut("Upload")]
        public async Task<ActionResult<ResponseEntity<Archivo>>> PutArchivobyId(IFormFile file, String item)
        {
            ResponseEntity<Archivo> response = new ResponseEntity<Archivo>();
            var context = _httpContextAccessor.HttpContext;
            var form = context.Request.Form;
            item = item ?? form["item"];
            //IFormFile file = (IFormFile)form["file"];
            //IFormFile file = form.Files["file"];
            Archivo archivo = JsonConvert.DeserializeObject<Archivo>(item);
            string rutaGuardar = Path.Combine("", "C:\\inetpub\\wwwroot\\distTulicencia\\upload");// produccion
            //string rutaGuardar = Path.Combine("", "C:\\files");

            try
            {
                string nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}_{file.FileName}";
                string rutaArchivo = Path.Combine(rutaGuardar, nombreArchivo);
                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                archivo.ar_nombre = "https://tulicenciapr.com/upload/" + nombreArchivo;
                response = await _archivoRepository.UpdateArchivo(archivo);
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }








        [HttpPost("caso1Upload")]
        public async Task<ActionResult<ResponseEntity<Archivo>>> PostArchivoPdfCaso(Archivo archivo)
        {
            ResponseEntity<Archivo> response = new ResponseEntity<Archivo>();
            var context = _httpContextAccessor.HttpContext;
            string rutaGuardar = Path.Combine("", "C:\\inetpub\\wwwroot\\distTulicencia\\upload");
            if (!Directory.Exists(rutaGuardar))
            {
                Directory.CreateDirectory(rutaGuardar);
            }
            try
            {
                string nombreArchivo = $"{DateTime.Now:yyyyMMddHHmmss}.pdf";
                string rutaArchivo = Path.Combine(rutaGuardar, nombreArchivo);
                byte[] pdfBytes = await DescargarPdf(archivo.ar_nombre);
                await System.IO.File.WriteAllBytesAsync(rutaArchivo, pdfBytes);
                archivo.ar_nombre = "https://tulicenciapr.com/upload/" + nombreArchivo;
                archivo.ar_pos = 6;
                archivo.ar_fecha = DateTime.Now;
                response = await _archivoRepository.AddArchivo(archivo);
            }
            catch (Exception ex)
            {
                response.message = ex.Message;
            }
            return response;
        }


        private async Task<byte[]> DescargarPdf(string urlPdf)
        {
            using (HttpClient client = new HttpClient())
            {
                Uri requestUri = new Uri("https://pdfmyurl.com/api?license=bj6us3grfpsI&url=" + urlPdf);
                HttpResponseMessage response = await client.GetAsync(requestUri);
                response.EnsureSuccessStatusCode();
                byte[] pdfBytes = await response.Content.ReadAsByteArrayAsync();
                return pdfBytes;
            }
        }

        private void GuardarPdfEnBaseDeDatos(byte[] pdfBytes)
        {
            // Guardar los bytes del PDF en la base de datos
            string connectionString = "TuCadenaDeConexion"; // Cambia esto por tu cadena de conexión a la base de datos
            string query = "INSERT INTO Pdfs (PdfContent) VALUES (@PdfContent)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@PdfContent", pdfBytes);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }


    }
}
