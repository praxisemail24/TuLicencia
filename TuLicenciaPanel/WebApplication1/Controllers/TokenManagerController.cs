using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Authorize(Policy = "ElevatedRights", Roles = "Administrador")]
    public class TokenManagerController : Controller
    {
        private readonly ILogger<TokenManagerController> _logger;

        public TokenManagerController(ILogger<TokenManagerController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
