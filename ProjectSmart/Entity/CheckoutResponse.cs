namespace SmartLicencia.Entity
{
    public class CheckoutResponse
    {
        public object? Session { get; set; }
        public object? Data { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? PaymentId { get; set; }
        public string? PaymentStatus { get; set; }
        public bool WithErrors { get; set; }
        public string? Error { get; set; }
    }
}
