using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IMotivoRepository
    {
        Task<ResponseEntity<Motivo>> GetAllMotivo();
        Task<ResponseEntity<Motivo>> GetMotivoById(int id);
    }
}
