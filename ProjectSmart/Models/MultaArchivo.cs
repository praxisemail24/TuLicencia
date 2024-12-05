namespace SmartLicencia.Models
{
    public class MultaArchivo
    {
        public int Id { get; set; }
        public int TramiteId { get; set; }
        public int FormularioId { get; set; }
        public string NombreArchivo { get; set; }
        public string RootPath { get; set; }
        public string Url { get; set; }
        public string MimeType { get; set; }
        public long Tamanio { get; set; }
        public int? MultaId { get; set; }
        public int AutorId { get; set; }
        public string Origen { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? UltimaActualizacion { get; set; }

        public MultaArchivo()
        {
            NombreArchivo = string.Empty;
            RootPath = string.Empty;
            Origen = string.Empty;
            Url = string.Empty;
            MimeType = string.Empty;
        }
    }
}
