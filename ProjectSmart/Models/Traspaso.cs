using Newtonsoft.Json;

namespace SmartLicencia.Models
{
    public class Traspaso
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("pgId")]
        public int? PgId { get; set; }

        [JsonProperty("tprId")]
        public int? TprId { get; set; }

        [JsonProperty("trId")]
        public int? TrId { get; set; }

        [JsonProperty("brandVehicle")]
        public string? BrandVehicle { get; set; }

        [JsonProperty("modelVehicle")]
        public string? ModelVehicle { get; set; }

        [JsonProperty("yearVehicle")]
        public int? YearVehicle { get; set; }

        [JsonProperty("colorVehicle")]
        public string? ColorVehicle { get; set; }

        [JsonProperty("serialNumber")]
        public string? SerialNumber { get; set; }

        [JsonProperty("otherInfo")]
        public string? OtherInfo { get; set; }

        [JsonProperty("licensePlate")]
        public string? LicensePlate { get; set; }

        [JsonProperty("transferType")]
        public string? TransferType { get; set; }

        [JsonProperty("contractDate")]
        public DateTime? ContractDate { get; set; }

        [JsonProperty("hasContract")]
        public bool HasContract { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("state")]
        public int State { get; set; }

        [JsonProperty("paymentAmount")]
        public decimal? PaymentAmount { get; set; }

        [JsonProperty("paymentDate")]
        public DateTime? PaymentDate { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("reviewedAt")]
        public DateTime? ReviewedAt { get; set; }

        [JsonProperty("processedAt")]
        public DateTime? ProcessedAt { get; set; }

        [JsonProperty("closedAt")]
        public DateTime? ClosedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("seller")]
        public Cliente Seller { get; set; }

        [JsonProperty("buyer")]
        public Cliente Buyer { get; set; }
        public bool? RevisedStatus { get; set; }
        public DateTime? RevisedStatusAt { get; set; }
        public bool? EvaluationStatus { get; set; }
        public DateTime? EvaluationStatusAt { get; set; }
        public int? AdminId { get; set; }
        public int? RadicatorId { get; set; }
        public DateTime? RadicatorAsignatedAt { get; set; }
        public bool? RadicatedStatus { get; set; }
        public DateTime? RadicatedStatusAt { get; set; }
        public int? RadicationState { get; set; }
        public string? RadicationObservation { get; set; }
        public string? AdminName { get; set; }
        public string? RadicatorName { get; set; }

        [JsonProperty("documents")]
        public List<TraspasoDoc> Documents { get; set; }
        public int AuthorId { get; set; }

        public Traspaso()
        {
            Id = 0;
            Currency = "USD";
            Seller = new Cliente();
            Buyer = new Cliente();
            State = 0;
            Documents = new List<TraspasoDoc>();
        }
    }

    public class TraspasoDoc: Archivo
    {
        public string? ar_url { get; set; }

        public static string GetFileName(int pos)
        {
            if (pos == 1)
                return "Copia del licencia del vehículo";
            else if (pos == 2)
                return "Copia de licencia de conducir de vendedor";
            else if (pos == 3)
                return "Copia de licencia de conducir del comprador";
            else if (pos == 4)
                return "Título de propiedad (Opcional)";
            else if (pos == 5)
                return "Contrato de compraventa (Bill of Sale)";
            else if (pos == 6)
                return "Declaración jurada (Opcional)";
            else
                return string.Empty;
        }
    }
}
