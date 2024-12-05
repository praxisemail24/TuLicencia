using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IEstadosRepository
    {
        Task<ResponseEntity<Estados>> GetAllEstados();
        //Task<ResponseEntity<Pueblos>> GetPueblosById(int id);

    }
}
