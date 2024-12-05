using Microsoft.AspNetCore.Mvc;
using SmartLicense.Pdf;

namespace SmartLicencia.Controllers
{
    public class BuilderPDFController : Controller
    {
        protected readonly IConfiguration _config;
        protected readonly IWebHostEnvironment _hostEnv;

        public BuilderPDFController(IWebHostEnvironment host, IConfiguration config)
        {
            _hostEnv = host;
            _config = config;

            PDF.Initialize(new PDF.PdfConfig
            {
                Debug = config.GetValue<bool>("PdfGenerator:debug"),
                Bin = config.GetValue<string>("PdfGenerator:bin"),
                TempFolderPath = config.GetValue<string>("PdfGenerator:tempFolderPath"),
                CombinerBin = config.GetValue<string>("PdfCombiner:bin"),
            });
        }

        protected FileStreamResult renderPdf(string fileName, bool download = false)
        {
            var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var result = new FileStreamResult(fileStream, "application/pdf")
            {
                LastModified = DateTime.Now,
                EnableRangeProcessing = true,
            };

            if (download)
                result.FileDownloadName = Path.GetFileName(fileName);

            Response.Headers.Add("Content-Disposition", $"{(download ? "attachment" : "inline")}; filename=\"{Path.GetFileName(fileName)}\"");
            Response.Headers.Add("Content-Type", "application/pdf");

            return result;
        }

        protected async Task<string> GeneratePdfOfTemplate(string template, string fileName, IPdfModel param)
        {
            var templatePath = Path.Combine(_hostEnv.WebRootPath, $"..\\Templates\\{template}.tt");

            var html = await PDF.BuilderHtml(templatePath, param);

            PDF.Generate(html, fileName);

            return fileName;
        }
    }
}
