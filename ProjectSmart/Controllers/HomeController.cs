using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly HomeRepository __homeRepository;
        public HomeController(IConfiguration configuration)
        {
            __homeRepository = new HomeRepository(configuration);
        }

        [HttpPost("dashboard/tramites")]
        public DataTableJS<ReporteTramiteCaso> TramitesInicio([FromForm] DataTableJSRequest<TramiteDashboardRequest> dataTable)
        {
            var response = new DataTableJS<ReporteTramiteCaso>();
            try
            {
                var request = dataTable.AdditionalValues;
                var data = __homeRepository.TramitesInicio(request.AdminId ?? 0, request.Estado ?? 0, request.CodigoPago, request.TipoTramite, request.NombreTramite,request.EstadoProceso);
                var dataPage = data.Skip(dataTable.Start).Take(dataTable.Length);

                response.Data = dataPage;
                response.Draw = dataTable.Draw;
                response.RecordsTotal = data.Count();
                response.RecordsFiltered = data.Count();
            }
            catch (Exception ex)
            {
                response.Error = ex.Message;
            }
            return response;
        }
    }
}
