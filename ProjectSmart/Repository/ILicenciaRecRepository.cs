using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface ILicenciaRecRepository
    {
        Task<ResponseEntity<LicenciaRec>> GetAllLicenciaRec();
        Task<ResponseEntity<LicenciaRec>> GetLicenciaRecById(int id);
        Task<ResponseEntity<LicenciaRec>> AddLicenciaRec(LicenciaRec licenciaRec);
        Task<ResponseEntity<LicenciaRec>> UpdateLicenciaRec(LicenciaRec id);
        Task<ResponseEntity<LicenciaRec>> DeleteLicenciaRec(int id);
    }
}
