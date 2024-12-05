using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "ElevatedRights")]
    public class ClienteController : Controller
    {
        private readonly ILogger<ClienteController> _logger;

        public ClienteController(ILogger<ClienteController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
