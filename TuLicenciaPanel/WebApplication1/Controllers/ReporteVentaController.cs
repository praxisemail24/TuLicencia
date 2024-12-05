using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "ElevatedRights")]
    public class ReporteVentaController : Controller
    {
        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
