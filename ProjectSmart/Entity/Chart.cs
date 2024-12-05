using ClosedXML.Excel;
using Newtonsoft.Json;

namespace SmartLicencia.Entity
{
    public class Chart
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("data")]
        public ChartData Data { get; set; }

        [JsonProperty("options")]
        public dynamic Options { get; set; }

        public Chart() 
        {
            Type = "bar";
            Data = new ChartData();
            Options = new object();
        }
    }

    public class ChartData
    {
        [JsonProperty("labels")]
        public IEnumerable<string>? Labels { get; set; }

        [JsonProperty("datasets")]
        public IEnumerable<DataSetChart>? Datasets { get; set; }
    }

    public class DataSetChart
    {
        [JsonProperty("label")]
        public string? Label { get; set; }

        [JsonProperty("data")]
        public IEnumerable<decimal>? Data { get; set; }

        [JsonProperty("borderColor")]
        public object BorderColor { get; set; }

        [JsonProperty("backgroundColor")]
        public object? BackgroundColor { get; set; }

        [JsonProperty("fill")]
        public bool Fill { get; set; }

        public DataSetChart()
        {
            Fill = false;
            BorderColor = "#dedede";
            BackgroundColor = "#222222";
        }
    }
}
