using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "ElevatedRights")]
    public class ReporteCasoController : Controller
    {
        [Authorize(Roles = "Administrador")]
        public ActionResult General()
        {
            return View();
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult PorPeriodo()
        {
            return View();
        }


        [Authorize(Roles = "Administrador")]
        public ActionResult PorTiempo()
        {
            return View();
        }
    }
}
