namespace SmartLicencia.Services
{
    public class ResultadoPago
    {
        public string InvoiceNumber { get; set; }
        public string ResptaTransId { get; set; }

        public ResultadoPago()
        {
            InvoiceNumber = string.Empty;
            ResptaTransId = string.Empty;
        }
    }
}
