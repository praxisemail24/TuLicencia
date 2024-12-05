namespace SmartLicencia.Models
{
    public class PlantillaMensaje
    {
        public int Id { get; set; }
        public string CodigoInterno { get; set; }
        public string Asunto { get; set; }
        public string Cc { get; set; }
        public string Contenido { get; set; }
        public string Tipo { get; set; }

        public PlantillaMensaje() 
        { 
            CodigoInterno = string.Empty;
            Asunto = string.Empty;
            Cc = string.Empty;
            Contenido = string.Empty;
            Tipo = string.Empty;
        }
    }
}
