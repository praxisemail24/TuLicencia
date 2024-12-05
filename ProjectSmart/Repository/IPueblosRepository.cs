using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IPueblosRepository
    {
        Task<ResponseEntity<Pueblos>> GetAllPueblos();
        Task<ResponseEntity<Pueblos>> GetPueblosById(int id);

    }
}
