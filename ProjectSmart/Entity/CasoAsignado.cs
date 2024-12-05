namespace SmartLicencia.Entity
{
    public class CasoAsignado
    {
        public long RowIndex { get; set; }
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Codigo { get; set; }
        public string NombreCliente { get; set; }
        public string TipoTramite { get; set; }
        public string NombreTramite { get; set; }
        public string Formulario { get; set; }
        public string Evaluacion { get; set; }
        public string Estado { get; set; }
        public int tr_id { get; set; }


    }
}
