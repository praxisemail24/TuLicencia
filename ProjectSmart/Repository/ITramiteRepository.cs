using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface ITramiteRepository
    {
        Task<ResponseEntity<Tramite>> GetAllTramite();
        IEnumerable<Tramite> ListarTramites();

    }
}
