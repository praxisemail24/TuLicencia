namespace SmartLicencia.Models
{
    public class RecordChoferil
    {
        public long Id { get; set; }
        public int ClId { get; set; }
        public int PgId { get; set; }
        public DateTime? CertifiedAt { get; set; }
        public string? CertifiedType { get; set; }
        public decimal CertifiedPrice { get; set; }
        public string Lang { get; set; }
        public string? LicensePlate { get; set; }
        public string? Category { get; set; }
        public string? Number { get; set; }
        public string? Serie { get; set; }
        public DateTime? ExpeditionAt { get; set; }
        public DateTime? ExpirationAt { get; set; }
        public string? PurposeApplication { get; set; }


        public RecordChoferil()
        {
            Lang = "es";
        }
    }
}
