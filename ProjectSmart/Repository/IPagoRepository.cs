using SmartLicencia.Entity;
using SmartLicencia.Models;
using SmartLicencia.Services;

namespace SmartLicencia.Repository
{
    public interface IPagoRepository
    {
        Task<ResponseEntity<Pago>> Add(Pago Cliente, ResultadoPago resptaTransId);
        Task<ResponseEntity<Pago>> AddPagoMulta(Pago Cliente, ResultadoPago resptaTransId);
        Task<ResponseEntity<Pago>> GetPagoByIdCliente(int id);
        Task<ResponseEntity<Pago>> GetAllPago();
        Task<ResponseEntity<Pago>> GetPagoByCodigoPago(string pg_codigo);
        Task<ResponseEntity> GenerarReciboDePagoPDF(string pg_codigo, string nombreArchivo);
        ResponseEntity GenerarReciboDePagoPDFM(string nombreArchivo, Pago pago);
        Task<ResponseEntity<Pago>> GetEstadoByPago(string pg_codigo);
        Task<ResponseEntity> GenerarEvaluacionMedicaPDF(string idEvaluacion,string tr_id, string nombreArchivo, EvaluacionRequest evaluacion);

        Task<Pago> GetPagoPorTransId(string transId);
    }
}
