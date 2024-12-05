namespace SmartLicencia.Models
{
    public class Tramite
    {
        public int tr_id { get; set; }        
        public string tr_nombre { get; set; }
        public int tr_estado { get; set; }
        public decimal tr_precio { get; set; }
        public TipoTramite tr_tipoTramite { get; set; }

        public string tipoparcialtotal { get; set; }

    }
}
