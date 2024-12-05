using System.ComponentModel;

namespace SmartLicencia.Models
{
    public class ReporteTramiteCasoTiempo
    {
        [DisplayName("ID")]

        public long Id { get; set; }

        [DisplayName("TIPO TRÁMITE")]
        public string? TipoTramite { get; set; }

        [DisplayName("NOMBRE CLIENTE")]
        public string? NombreCliente { get; set; }

        [DisplayName("CORREO")]
        public string? Correo { get; set; }

        [DisplayName("TELEFONO")]
        public string? Telefono { get; set; }

        [DisplayName("CÓDIGO PAGO")]
        public string? PagoCodigo { get; set; }

        [DisplayName("FECHA PAGO")]
        public DateTime? PagoFecha { get; set; }

        [DisplayName("ESTADO")]
        public string? Estado { get; set; }

        [DisplayName("ESTADO PROCESO")]
        public string? EstadoProceso { get; set; }

        public int? ClienteId { get; set; }
        public int? PagoId { get; set; }

        [DisplayName("DÍAS")]
        public int Dias { get; set; }
    }
}
