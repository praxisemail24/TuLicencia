using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface ITipoTramiteRepository
    {
        Task<ResponseEntity<TipoTramite>> GetAllTipoTramite();
    }
}
