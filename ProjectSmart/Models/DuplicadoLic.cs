namespace SmartLicencia.Models
{
    public class DuplicadoLic
    {
        public Cliente cl_cliente { get; set; }
        public Pago pg_pago { get; set; }
        public Tramite tr_tramite { get; set; }

        public int fdl_id { get; set; }
        public DateTime fdl_fecha { get; set; }
        public int fdl_estado { get; set; }
        public string fdl_tipoLicencia { get; set; }
        public string fdl_numeroLicencia { get; set; }
        public string fdl_categoria { get; set; }
        public string fdl_vehiculoPesado { get; set; }
        public string fdl_identificacion { get; set; }
        public string fdl_numero { get; set; }
        public string fdl_StatusLegal { get; set; }
        public string fdl_genero { get; set; }
        public string fdl_donante { get; set; }
        public string fdl_tipoSangre { get; set; }
        public string fdl_talla { get; set; }
        public string fdl_peso { get; set; }
        public string fdl_tez { get; set; }
        public string fdl_colorPelo { get; set; }
        public string fdl_colorOjo { get; set; }
        public string fdl_direccion { get; set; }
        public string fdl_numeroDireccion { get; set; }
        public string fdl_pueblo { get; set; }
        public string fdl_codigoPostal { get; set; }
        public string fdl_barrio { get; set; }
        public string fdl_apartado { get; set; }
        public string fdl_pueblo2 { get; set; }
        public string fdl_codigoPostal2 { get; set; }
        public string fdl_licenciaSuspendida { get; set; }
        public string fdl_motivoSuspension { get; set; }
        public string fdl_recluido { get; set; }
        public string fdl_convictoBebida { get; set; }
        public DateTime? fdl_fechaConvictoBebida { get; set; }
        public string fdl_convictoNarcotico { get; set; }
        public DateTime? fdl_fechaConvictoNarcotico { get; set; }
        public string fdl_obligacionAlimentaria { get; set; }
        public string fdl_deudaAcca { get; set; }

        public string? fdl_paisProcede { get; set; }
    }
}
