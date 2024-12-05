namespace SmartLicencia.Entity
{
    public class FiltroCasosRequest
    {
        public int DoctorId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string NombreTramite { get; set; }
        public string NombreCliente { get; set; }
        public string Estado { get; set; }
    }
}
