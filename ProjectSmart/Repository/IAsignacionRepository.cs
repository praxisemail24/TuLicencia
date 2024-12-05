using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IAsignacionRepository
    {
        Task<ResponseEntity<Asignacion>> AddAsignacion(Asignacion asignacion);
        Task<ResponseEntity<Asignacion>> AddAsignacionDoctor(Asignacion asignacion);
        Task<ResponseEntity<Asignacion>> GetDatosAsignacion(int frm_id, int tr_id);

        Task<IEnumerable<LineaTiempoTramite>> ObtenerLineaTiempo(int frm_id, int tr_id);
    }
}
