using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Repository;
using Swashbuckle.AspNetCore.Annotations;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Contralador de inicio.")]
    public class HomeController : Controller
    {
        private readonly HomeRepository __homeRepository;
        public HomeController(IConfiguration configuration)
        {
            __homeRepository = new HomeRepository(configuration);
        }

        [HttpPost("dashboard/tramites")]
        [SwaggerOperation(
            Summary = "Lista de trámites.",
            Description = "Listado de trámites de inicio por caso.",
            OperationId = "TramitesInicio",
            Tags = new string[] { "Home" }
        )]
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
