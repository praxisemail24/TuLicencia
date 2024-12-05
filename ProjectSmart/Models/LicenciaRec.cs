namespace SmartLicencia.Models
{
    public class LicenciaRec    // Frm_LicenciaReciprocidad
    {
        public Cliente cl_cliente { get; set; }
        public Pago pg_pago { get; set; }
        public Tramite tr_tramite { get; set; }

        public int flr_id { get; set; }
        public DateTime flr_fecha { get; set; }
        public int flr_estado { get; set; }
        public string flr_tipoLicencia { get; set; }
        public string flr_numeroLicencia { get; set; }
        public string flr_categoria { get; set; }
        public string flr_tipoVehiculo { get; set; }
        public string flr_paisProcede { get; set; }
        public string flr_estadoProcede { get; set; }
        public string flr_numeroLicencia2 { get; set; }
        public DateTime flr_fechaExpiracion { get; set; }
        public string flr_identificacion { get; set; }
        public string flr_numeroIdentificacion { get; set; }
        public string flr_statusLegal { get; set; }
        public string flr_genero { get; set; }
        public string flr_donante { get; set; }
        public string flr_tipoSangre { get; set; }
        public string flr_talla { get; set; }
        public string flr_peso { get; set; }
        public string flr_tez { get; set; }
        public string flr_colorPelo { get; set; }
        public string flr_colorOjo { get; set; }
        public string flr_nombrePadre { get; set; }
        public string flr_nombreMadre { get; set; }
        public string flr_direccion { get; set; }
        public string flr_numeroDireccion { get; set; }
        public string flr_pueblo { get; set; }
        public string flr_codigoPostal { get; set; }
        public string flr_barrio { get; set; }
        public string flr_apartado { get; set; }
        public string flr_pueblo2 { get; set; }
        public string flr_codigoPostal2 { get; set; }
        public string flr_licenciaSuspendida { get; set; }
        public string flr_motivoSuspencion { get; set; }
        public string flr_numeroLicenciaPR { get; set; }
        public string flr_recluido { get; set; }
        public string flr_convictoBebida { get; set; }
        public DateTime? flr_fechaConvictoBebida { get; set; }
        public string flr_convictoNarcotico { get; set; }
        public DateTime? flr_fechaConvictoNarcotico { get; set; }
        public string flr_obligacionAlimentaria { get; set; }
        public string flr_deudaAcca { get; set; }

    }
}
