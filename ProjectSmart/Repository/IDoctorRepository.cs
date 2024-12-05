using SmartLicencia.Controllers;
using SmartLicencia.Entity;
using SmartLicencia.Models;

namespace SmartLicencia.Repository
{
    public interface IDoctorRepository
    {
        List<CasoAsignado> ObtenerCasosPorDoctorYFiltros(FiltroCasosRequest filtro);
        //Task GuardarEvaluacion(EvaluacionRequest request);
        
        //Task ObtenerEvaluacionPorId(int id);

        Task<EvaluacionRequest> ObtenerEvaluacionPorId(int id, int tr_id);
        Task GuardarEvaluacion(EvaluacionRequest request);
    }
}
