using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WebApplication1.Models;
    
namespace WebApplication1.Controllers
{
    [Authorize(Policy = "ElevatedRights", Roles = "Administrador")]
    public class AdministradorController : Controller
    {
        private readonly ILogger<AdministradorController> _logger;

        public AdministradorController(ILogger<AdministradorController> logger)
        {
           _logger = logger;
        }

        [Authorize(Roles = "Administrador")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
