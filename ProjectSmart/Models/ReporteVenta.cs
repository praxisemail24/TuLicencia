using SmartLicencia.Entity;

namespace SmartLicencia.Models
{
    public class ReporteVenta
    {
        public string? TipoTramite { get; set; }
        public string Grupo { get; set; }
        public int Cantidad { get; set; }
        public double Monto { get; set; }

        public ReporteVenta()
        {
            Grupo = string.Empty;
        }
    }

    public class ReporteVentaGrupo
    {
        public int Cantidad { get; set; }
        public decimal Monto { get; set; }
        public List<ReporteVenta> Lista { get; set; }
        public DataSetChart DataSetMonto { get; set; }
        public DataSetChart DataSetCantidad { get; set; }

        public ReporteVentaGrupo()
        {
            Lista = new List<ReporteVenta>();
            DataSetMonto = new DataSetChart();
            DataSetCantidad = new DataSetChart();
        }
    }
}
