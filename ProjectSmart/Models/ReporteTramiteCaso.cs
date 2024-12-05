using SmartLicencia.Entity;
using System.ComponentModel;

namespace SmartLicencia.Models
{
    public class ReporteTramiteCaso
    {
        [DisplayName("ID")]

        public long Id { get; set; }

        [DisplayName("TIPO TRÁMITE")]
        public string? TipoTramite { get; set; }
        
        [DisplayName("NOMBRE TRÁMITE")]
        public string? NombreTramite { get; set; }


        [DisplayName("NOMBRE CLIENTE")]
        public string? NombreCliente { get; set; }

        [DisplayName("StatusEvaluacion")]
        public string? StatusEvaluacion { get; set; }

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

  
        [DisplayName("estadoProceso")]
        public int? estadoProceso { get; set; }
        public string? estadoProcesoText { get; set; }

        [DisplayName("Doctor")]
        public string? Doctor { get; set; }

        public int? ClienteId { get; set; }
        public int? PagoId { get; set; }
        public int? TrId { get; set; }
    }

    public class TramiteCasoGrupo
    {
        public int Mes { get; set; }
        public string? Grupo { get; set; }
        public decimal Cantidad { get; set; }
    }

    public class ChartTramiteCasoGrupo
    {
        public List<TramiteCasoGrupo> Lista { get; set; }
        public decimal Monto { get; set; }
        public DataSetChart DataSet { get; set; }

        public ChartTramiteCasoGrupo()
        {
            Lista = new List<TramiteCasoGrupo>();
            DataSet = new DataSetChart();
        }
    }
}
