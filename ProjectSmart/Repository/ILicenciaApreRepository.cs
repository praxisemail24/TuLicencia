using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface ILicenciaApreRepository
    {
        Task<ResponseEntity<LicenciaApre>> GetAllLicenciaApre();
        Task<ResponseEntity<LicenciaApre>> GetLicenciaApreById(int id);
        Task<ResponseEntity<LicenciaApre>> AddLicenciaApre(LicenciaApre licenciaApre);
        Task<ResponseEntity<LicenciaApre>> UpdateLicenciaApre(LicenciaApre id);
        Task<ResponseEntity<LicenciaApre>> DeleteLicenciaApre(int id);
    }
}
