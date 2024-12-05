using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface ITabEspecialesRepository
    {
        Task<ResponseEntity<TabEspeciales>> AddTabEspeciales(TabEspeciales tabEspeciales);
    }
}
