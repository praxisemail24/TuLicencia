using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class MultaController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VerPago(string codigopago)
        {
            ViewBag.CodigoPago = codigopago;
            return View("verpago");
        }
    }
}
