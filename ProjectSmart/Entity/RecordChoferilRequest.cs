using Swashbuckle.AspNetCore.Annotations;

namespace SmartLicencia.Entity
{
    public class RecordChoferilRequest
    {
        [SwaggerSchema("Identificador de registro.")]
        public long Id { get; set; }

        [SwaggerSchema("Identificador de cliente.")]
        public int ClId { get; set; }

        [SwaggerSchema("Identificador de pago.")]
        public int PgId { get; set; }

        [SwaggerSchema("Idioma de la información: Español (es) - Ingles (en).")]
        public string Lang { get; set; }

        [SwaggerSchema("Tipo de record choferil: Conductor (conductor) - Vehículo (vehiculo).")]
        public string? Type { get; set; }

        [SwaggerSchema("Placa del vehículo.")]
        public string? LicensePlate { get; set; }

        [SwaggerSchema("Categoria.")]
        public string? Category { get; set; }

        [SwaggerSchema("Número de registro.")]
        public string? Number { get; set; }

        [SwaggerSchema("Serie.")]
        public string? Serie { get; set; }

        [SwaggerSchema("Propósito de solicitud.")]
        public string? PurposeApplication { get; set; }
        public DateTime? ExpeditionAt { get; set; }
        public DateTime? ExpirationAt { get; set; }

        [SwaggerSchema("Firma del solicitante en base64.")]
        public string? SignatureApplicant { get; set; }

        public RecordChoferilRequest()
        {
            Lang = "es";
        }
    }
}
