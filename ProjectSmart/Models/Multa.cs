using System.ComponentModel;

namespace SmartLicencia.Models
{
    public class Multa
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public int FormularioId { get; set; }
        public int ClienteId { get; set; }
        public string Origen { get; set; }

        [DisplayName("DESCRIPCIÓN")]
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public int AutorId { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimaActualizacion { get; set; }


        [DisplayName("MONTO")]
        public string MontoStr
        {
            get
            {
                return string.Format("$ {0:0.00}", Monto);
            }
        }

        public Multa()
        {
            Origen = string.Empty;
            Descripcion = string.Empty;
            Estado = string.Empty;
        }
    }
}
