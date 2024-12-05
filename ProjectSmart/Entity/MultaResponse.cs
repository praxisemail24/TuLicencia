using SmartLicencia.Models;

namespace SmartLicencia.Entity
{
    public class MultaResponse
    {
        public string NombreCliente { get; set; }
        public string NroLicencia { get; set; }
        public string NroSSN { get; set; }

        public string Correo { get; set; }

        public string PagoCesco { get; set; }
        public string PagoAutoExpress { get; set; }
        public string TipoPago { get; set; }

        private DateTime? _fechaNac;
        public DateTime? FechaNacValue 
        {
            set { _fechaNac = value; }
        }
        public string FechaNac
        {
            get
            {
                if (_fechaNac == null)
                    return string.Empty;

                return string.Format("{0:MM/dd/yyyy}", _fechaNac);
            }
        }

        public IEnumerable<Multa> Multas { get; set; }
        public IEnumerable<MultaArchivo> Archivos { get; set; }
        public MultaPago? Pago { get; set; }
        public string? Error { get; set; }

        public bool ConPagoTotal
        {
            get
            {
                return Pago?.Origen == "total";
            }
        }

        public MultaResponse()
        {
            NombreCliente = string.Empty;
            NroLicencia = string.Empty;
            NroSSN = string.Empty;
            Multas = new List<Multa>();
            Archivos = new List<MultaArchivo>();
        }
    }
}
