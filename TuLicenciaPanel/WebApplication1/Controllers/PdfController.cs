using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using static System.Net.Mime.MediaTypeNames;

namespace WebApplication1.Controllers
{
    public class PdfController : Controller
    {
        public IActionResult Viewer([FromQuery] PDFViewModel pdfViewModel)
        {
            ViewData.Model = pdfViewModel;
            return View();
        }
    }
}
