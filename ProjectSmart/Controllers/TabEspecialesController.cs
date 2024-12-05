using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TabEspecialesController : ControllerBase
    {
        private readonly ITabEspecialesRepository _tabEspecialesRepository;

        public TabEspecialesController(ITabEspecialesRepository tabEspecialesRepository)
        {
            _tabEspecialesRepository = tabEspecialesRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseEntity<TabEspeciales>>> PostTabEspeciales(TabEspeciales tabEspeciales)
        {
            var response = await _tabEspecialesRepository.AddTabEspeciales(tabEspeciales);
            return Ok(response);
        }
    }
}
