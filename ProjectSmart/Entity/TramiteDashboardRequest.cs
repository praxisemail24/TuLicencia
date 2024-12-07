using Swashbuckle.AspNetCore.Annotations;

namespace SmartLicencia.Entity
{
    public class TramiteDashboardRequest
    {
        [SwaggerSchema("Identificador de administrador.")]
        public int? AdminId { get; set; }

        [SwaggerSchema("Filtro estado de trámite.")]
        public int? Estado { get; set; }

        [SwaggerSchema("Filtro de tipo de trámite.")]
        public int? TipoTramite { get; set; }

        [SwaggerSchema("Filtro nombre de trámite.")]
        public string? NombreTramite { get; set; }

        [SwaggerSchema("Filtro código de pago.")]
        public string? CodigoPago { get; set; }

        [SwaggerSchema("Filtro estado de proceso.")]
        public string? EstadoProceso { get; set; }
    }
}
