using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartLicencia.Entity;
using SmartLicencia.Repository;

namespace SmartLicencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationController : Controller
    {
        private readonly NotificationRepository _notificationRepository;

        public NotificationController(IConfiguration configuration) {
            _notificationRepository = new NotificationRepository(configuration);
        }

        [HttpPost("{user_id}/tramites")]
        public ResponseJSON Index(int user_id)
        {
            var response = new ResponseJSON();
            try
            {
                response.Success = true;
                response.Data = _notificationRepository.UltimosTramites(user_id);
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
