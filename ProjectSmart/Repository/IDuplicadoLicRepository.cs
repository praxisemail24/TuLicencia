using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IDuplicadoLicRepository
    {
        Task<ResponseEntity<DuplicadoLic>> GetAllDuplicadoLic();
        Task<ResponseEntity<DuplicadoLic>> GetDuplicadoLicById(int id);
        Task<ResponseEntity<DuplicadoLic>> AddDuplicadoLic(DuplicadoLic duplicadoLic);
        Task<ResponseEntity<DuplicadoLic>> UpdateDuplicadoLic(DuplicadoLic id);
        Task<ResponseEntity<DuplicadoLic>> DeleteDuplicadoLic(int id);
    }
}
