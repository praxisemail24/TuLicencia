using Org.BouncyCastle.Bcpg;

namespace SmartLicencia.Models
{
    public class Pago
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumeroTarjeta { get; set; }        
        public string Cvv { get; set; }
        public string FechaVencimiento { get; set; }
        public decimal Monto { get; set; }
        public string Description {  get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Direction { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public int pg_id { get; set; }
        public DateTime pg_fecha { get; set; }
        public string pg_codigo { get; set;}
        public string pg_status { get; set;}
        public string pg_nota { get; set; }
        public int pg_estado { get; set; }
        public int pg_metodo { get; set; }        
        public string pg_txid { get; set; }

        public string tipo { get; set; }
        public string origen { get; set; }
        public string cuotas { get; set; }
        
        public string frm_id { get; set; }

        public Cliente cl_cliente { get; set; }
        public Tramite tr_tramite { get; set; }

        public  List<Cuota> DetalleCuota { get; set; }

    }

    public class Cuota
    {
        public string nro { get; set; }
        public string monto { get; set; }
        public string fecha { get; set; }
    }

}
