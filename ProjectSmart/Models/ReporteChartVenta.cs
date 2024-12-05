using SmartLicencia.Entity;

namespace SmartLicencia.Models
{
    public class ReporteChartVenta
    {
        public Chart AmountBar { get; set; }
        public Chart QuantityBar { get; set; }
        public int Total { get; set; }
        public decimal Monto { get; set; }

        public ReporteChartVenta()
        {
            AmountBar = new Chart();
            QuantityBar = new Chart();
        }
    }
}
