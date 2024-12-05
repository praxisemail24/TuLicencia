namespace SmartLicencia.Entity
{
    public class ReporteCasoRequest
    {
        public int TipoTramite { get; set; }
        public int? EstadoTipo { get; set; }
        public int? EstadoProceso { get; set; }
        public string? Nombres { get; set; }
        public string? ApellidoMaterno { get; set; }
        public string? ApellidoPaterno { get; set; }
        public string? Correo { get; set; }


        public int? TipoReporte { get; set; }
        public int? Mes { get; set; }
        public int? Anio { get; set; }
        public string? FechaInicio { get; set; }
        public string? FechaTermino { get; set; }


        public int NroDias { get; set; }


        public string? cl_nombre { get; set; }
        public string? cl_correo { get; set; }
        public string? cl_numeroTelefono { get; set; }
        public string? pg_codigo { get; set; }
        public string? tr_id { get; set; }
        public string? fecha { get; set; }
        public string? estado { get; set; }



    }
}
