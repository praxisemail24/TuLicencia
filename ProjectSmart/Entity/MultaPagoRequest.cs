namespace SmartLicencia.Entity
{
    public class MultaPagoRequest
    {
        public int TipoTramite { get; set; }
        public string NombreCliente { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Fecha { get; set; }
        public int Estado { get; set; }

        public MultaPagoRequest()
        {
            NombreCliente = string.Empty;
            Correo = string.Empty;
            Telefono = string.Empty;
            Fecha = string.Empty;
        }
    }
}
