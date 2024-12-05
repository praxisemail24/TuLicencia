using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IObservacionProcessRepository
    {
        Task<ResponseEntity<ObservacionProcess>> GetObservacionProcessById(int ob_id);
        Task<ResponseEntity<ObservacionProcess>> AddObservacionProcess(ObservacionProcess observacionProcess);

    }
}
