using SmartLicencia.Models;

namespace SmartLicencia.Entity
{
    public class TraspasoRequest
    {
        public long Id { get; set; }
        public int? PgId { get; set; }
        public string? BrandVehicle { get; set; }
        public string? ModelVehicle { get; set; }
        public int? YearVehicle { get; set; }
        public string? ColorVehicle { get; set; }
        public string? SerialNumber { get; set; }
        public string? OtherInfo { get; set; }
        public string? LicensePlate { get; set; }
        public string? TransferType { get; set; }
        public DateTime? ContractDate { get; set; }
        public bool HasContract { get; set; }
        public string Currency { get; set; }
        public int State { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateTime? PaymentDate { get; set; }

        public TraspasoClienteRequest Seller { get; set; }
        public TraspasoClienteRequest Buyer { get; set; }
        public int AuthorId { get; set; }

        public TraspasoRequest()
        {
            Id = 0;
            Currency = "USD";
            Seller = new TraspasoClienteRequest();
            Buyer = new TraspasoClienteRequest();
            State = 0;
        }
    }

    public class TraspasoClienteRequest
    {
        public Pueblos? cl_pueblo { get; set; }
        public int cl_id { get; set; }
        public string? cl_nombre { get; set; }
        public string? cl_segundoNombre { get; set; }
        public string? cl_primerApellido { get; set; }
        public string? cl_segundoApellido { get; set; }
        public string? cl_nombreCompleto { get; set; }
        public string? cl_zip { get; set; }
        public string? cl_direccion { get; set; }
        public string? cl_numeroLicencia { get; set; }
        public string? cl_numeroSeguro { get; set; }
        public string? cl_numeroTelefono { get; set; }
        public string? cl_correo { get; set; }
    }

    public class TraspasosStatus
    {
        public long? Id { get; set; }
        public int? State { get; set; }
        public bool? Revised { get; set; }
        public bool? Evaluation { get; set; }
        public int? AdminId { get; set; }
        public int? RadicatorId { get; set; }
        public bool? Radicated { get; set; }
        public string? RadicationState { get; set; }
        public string? RadicationObservation { get; set; }
    }
}
