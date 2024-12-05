namespace SmartLicencia.Models
{
    public class RenovLic   // Frm_RenovacionLicencia
    {
        public Cliente cl_cliente { get; set; }
        public Pago pg_pago { get; set; }
        public Tramite tr_tramite { get; set; }

        public int frl_id { get; set; }
        public DateTime frl_fecha { get; set; }
        public int frl_estado { get; set; }
        public string frl_tipoLicencia { get; set; }
        public string frl_numeroLicencia { get; set; }
        public string frl_categoria { get; set; }
        public string frl_vehiculoPesado { get; set; }
        public string frl_identificacion { get; set; }
        public string frl_numero { get; set; }
        public string frl_StatusLegal { get; set; }
        public string frl_genero { get; set; }
        public string frl_donante { get; set; }
        public string frl_tipoSangre { get; set; }
        public string frl_talla { get; set; }
        public string frl_peso { get; set; }
        public string frl_tez { get; set; }
        public string frl_colorPelo { get; set; }
        public string frl_colorOjo { get; set; }
        public string frl_direccion { get; set; }
        public string frl_numeroDireccion { get; set; }
        public string frl_pueblo { get; set; }
        public string frl_codigoPostal { get; set; }
        public string frl_barrio { get; set; }
        public string frl_apartado { get; set; }
        public string frl_pueblo2 { get; set; }
        public string frl_codigoPostal2 { get; set; }
        public string frl_licenciaSuspendida { get; set; }
        public string frl_motivoSuspension { get; set; }
        public string frl_recluido { get; set; }
        public string frl_convictoBebida { get; set; }
        public DateTime? frl_fechaConvictoBebida { get; set; }
        public string frl_convictoNarcotico { get; set; }
        public DateTime? frl_fechaConvictoNarcotico { get; set; }
        public string frl_obligacionAlimentaria { get; set; }
        public string frl_deudaAcca { get; set; }
        public int? frl_estadoProceso { get; set; }
        public int? frl_estadoRevision { get; set; }
        public string? frl_paisProcede { get; set; }
    }
}
