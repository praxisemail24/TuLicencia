using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MultaPagoController : Controller
    {
        [HttpPost("Listar")]
        public DataTableJS<MultaPagoEntity> Index(DataTableJSRequest<MultaPagoRequest> dt)
        {
            var response = new DataTableJS<MultaPagoEntity>();
            try
            {

            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }
    }
}
