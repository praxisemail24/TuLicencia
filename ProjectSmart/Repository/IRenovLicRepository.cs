using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IRenovLicRepository
    {
        Task<ResponseEntity<RenovLic>> GetAllRenovLic();
        Task<ResponseEntity<RenovLic>> GetRenovLicById(int id);
        Task<ResponseEntity<RenovLic>> AddRenovLic(RenovLic renovLic);
        Task<ResponseEntity<RenovLic>> UpdateRenovLic(RenovLic id);
        Task<ResponseEntity<RenovLic>> DeleteRenovLic(int id);
        Task<ResponseEntity<RenovLic>> GetRenovLicValidacion(int cl_id, int pg_id, int tr_id);


        Task<ResponseEntity<FormularioDTO>> GetFormEstado0();
        Task<ResponseEntity<FormularioDTO>> GetFormEstado1();
        Task<ResponseEntity<FormularioDTO>> GetFormEstado2();
        Task<ResponseEntity<FormularioDTO>> GetFormEstado3();


        Task<ResponseEntity<FormularioDTO>> GetDatosCompletoForm(int id, int tr_id,int frm_id);
        Task<ResponseEntity<FormularioDTO>> UpdateDatosCompletoFormPanel(FormularioDTO formularioDTO);
        Task<ResponseEntity<int>> cambioEstadoForm(int tr_id, int frm_id, int frm_estado);
        Task<ResponseEntity<int>> cambioEstadoProcesoForm(int tr_id, int frm_id, int frm_estado,string motivo);


        Task<ResponseEntity<FormularioDTO>> GetObtenerRegistro1();
        Task<ResponseEntity<FormularioDTO>> GetBuscarReportePanel(FormularioDTO item, PaginatorEntity paginator);

        Task<ResponseEntity<FormularioDTO>> GetBuscarReportePanelExcel(FormularioDTO item);
    }

}
