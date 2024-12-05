using SmartLicencia.Entity;

namespace SmartLicencia.Models
{
    public class ReporteChartPeriodo
    {
        public Chart ChartBar { get; set; }
        public List<dynamic> Summary { get; set; }
        public decimal SinIniciar { get; set; }

        public ReporteChartPeriodo()
        {
            ChartBar = new Chart()
            {
                Data = new ChartData()
                {
                    Labels = new string[] { "TOTAL", },
                    Datasets = new List<DataSetChart>(),
                },
                Options = new
                {
                    plugins = new
                    {
                        title = new
                        {
                            display = true,
                            text = "ESTADO DE CASOS POR PERIODO",
                        }
                    }
                }
            };
            Summary = new List<dynamic>();
        }
    }
}
