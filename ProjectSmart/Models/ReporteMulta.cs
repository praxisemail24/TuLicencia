using DocumentFormat.OpenXml.Presentation;
using SmartLicencia.Entity;
using System.ComponentModel;

namespace SmartLicencia.Models
{
    public class ReporteMulta
    {

        [DisplayName("ID")]
        public string? ID { get; set; }

        [DisplayName("CLIENTE ID")]
        public string? cl_id { get; set; }

        [DisplayName("NOMBRE CLIENTE")]
        public string? nombreCliente            { get; set; }
     
        [DisplayName("CORREO CLIENTE")]
        public string? cl_correo                { get; set; }
        
        [DisplayName("NUMERO TELEFONO")]
        public string? cl_numeroTelefono        { get; set; }
        
        [DisplayName("TRAMITE")]
        
        public string? tr_nombre                { get; set; }
        
        [DisplayName("ORIGEN")]
        public string? ORIGEN { get; set; }

        [DisplayName("TOTAL")]
        public string? TOTAL                    { get; set; }
        [DisplayName("PAGADO")]
        public string? PAGADO                   { get; set; }
        [DisplayName("TIPO")]
        public string? TIPO { get; set; }
        [DisplayName("FECHA")]
        public string? FECHA_CREACION           { get; set; }
        [DisplayName("CODIGO PAGO")]
        public string? CODIGO_PAGO { get; set; }

        [DisplayName("CANTIDAD CUOTAS")]
        public string? CANTIDAD_CUOTAS { get; set; }
        

    }
}
