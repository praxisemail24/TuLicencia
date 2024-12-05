using SmartLicencia.Models;

namespace SmartLicencia.Entity
{
    public class MultaPagoEntity
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public int FormularioId { get; set; }
        public int ClienteId { get; set; }
        public string NombreCliente { get; set; }
        public string TipoTramite { get; set; }
        public int CantidadCuotas { get; set; }
        public int AutorId { get; set; }
        public decimal Total { get; set; }
        public string Origen { get; set; }
        public string CodigoPago { get; set; }
        public string Tipo { get; set; }
        public bool Pagado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public List<MultaCuota> Cuotas { get; set; }

        public MultaPagoEntity()
        {
            NombreCliente = string.Empty;
            TipoTramite = string.Empty;
            Origen = string.Empty;
            Tipo = string.Empty;
            CodigoPago = string.Empty;
            Cuotas = new List<MultaCuota>();
            Pagado = false;
        }
    }
}
