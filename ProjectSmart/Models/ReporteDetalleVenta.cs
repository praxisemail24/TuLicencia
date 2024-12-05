using System.ComponentModel;

namespace SmartLicencia.Models
{
    public class ReporteDetalleVenta
    {
        [DisplayName("ID")]

        public int Id { get; set; }

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
        public int? Estado { get; set; }

        [DisplayName("MÉTODO")]
        public int? Metodo { get; set; }

        [DisplayName("NOTA")]
        public string? Nota { get; set; }

        [DisplayName("PRECIO")]
        public decimal Precio { get; set; }

        [DisplayName("INICIADO")]
        public string? Iniciado { get; set; }

        public int? ClienteId { get; set; }
        public int? PagoId { get; set; }
    }
}
