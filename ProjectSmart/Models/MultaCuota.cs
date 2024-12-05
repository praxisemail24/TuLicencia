namespace SmartLicencia.Models
{
    public class MultaCuota
    {
        public long Id { get; set; }
        public int MultaPagoId { get; set; }
        public int NroCuota { get; set; }
        public DateTime? Fecha { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public MultaCuota()
        {
            Estado = "pendiente";
        }
    }
}
