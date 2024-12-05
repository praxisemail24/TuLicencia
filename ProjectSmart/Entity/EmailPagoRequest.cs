namespace SmartLicencia.Entity
{
    public class EmailPagoRequest
    {
        public int TramiteId { get; set; }
        public int FormularioId { get; set; }
        public int ClienteId { get; set; }
        public string Origen { get; set; }
        public int Correo { get; set; }

        public EmailPagoRequest()
        {
            Origen = string.Empty;
        }
    }
}
