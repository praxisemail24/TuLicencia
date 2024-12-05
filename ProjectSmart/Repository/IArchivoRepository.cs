using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IArchivoRepository
    {
        Task<ResponseEntity<Archivo>> AddArchivo(Archivo archivo);

        Task<ResponseEntity<Archivo>> GetArchivoByFrmId(int frm_id);

        Task<ResponseEntity<Archivo>> DeleteArchivoByFrmId(int ar_id);

        Task<ResponseEntity<Archivo>> UpdateArchivo(Archivo archivo);

        Task<ResponseEntity<Archivo>> AddArchivoPDFcaso1(Archivo archivo);

    }
}
