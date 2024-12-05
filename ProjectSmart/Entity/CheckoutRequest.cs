namespace SmartLicencia.Entity
{
    public class CheckoutRequest
    {
        public string PayloadUrl { get; set; }
        public string? Code { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
        public string? Currency { get; set; }
        public int ClId { get; set; }
        public int TrId { get; set; }
        public string? FrmId { get; set; }
        public string? PenaltyType { get; set; }
        public string? PenaltyOrigin { get; set; }
        public string Installments { get; set; }

        public CheckoutRequest()
        {
            PayloadUrl = string.Empty;
            Code = string.Empty;
            Installments = "1";
            Name = string.Empty;
        }
    }

    public class PaymentRequest
    {
        public string Name { get; set; }
        public string HolderName { get; set; }
        public string HolderEmail { get; set; }
        public decimal Amount { get; set; }
        public string? Currency { get; set; }
        public string Token { get; set; }
        public int ClId { get; set; }
        public int TrId { get; set; }
        public string? FrmId { get; set; }
        public string? PenaltyOrigin { get; set; }
        public string? PenaltyType { get; set; }
        public string? Installments { get; set; }

        public PaymentRequest()
        {
            Name = string.Empty;
            HolderName = string.Empty;
            HolderEmail = string.Empty;
            Token = string.Empty;
        }
    }
}
