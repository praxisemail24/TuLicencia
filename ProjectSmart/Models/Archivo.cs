namespace SmartLicencia.Models
{
    public class Archivo
    {
        public Cliente cl_cliente { get; set; }
        public Tramite tr_tramite { get; set; }
        public int frm_id { get; set; }
        public int ar_id { get; set; }
        public string ar_nombre { get; set; }
        public DateTime ar_fecha { get; set; }
        public int ar_estado { get; set; }
        public int ar_pos { get; set; }
        public int pg_id { get; set; }
    }
}
