namespace SmartLicencia.Entity
{
    public class TramiteDashboardRequest
    {
        public int? AdminId { get; set; }
        public int? Estado { get; set; }
        public int? TipoTramite { get; set; }
        public string? NombreTramite { get; set; }
        public string? CodigoPago { get; set; }
        public string? EstadoProceso { get; set; }
    }
}
