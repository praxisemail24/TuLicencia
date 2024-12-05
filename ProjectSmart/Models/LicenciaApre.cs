namespace SmartLicencia.Models
{
    public class LicenciaApre
    {
        public Cliente cl_cliente { get; set; }
        public Pago pg_pago { get; set; }
        public Tramite tr_tramite { get; set; }


        public int fla_id { get; set; }
        public DateTime fla_fecha { get; set; }
        public int fla_estado { get; set; }
        public string fla_tipoLicencia { get; set; }
        public string fla_identificacion { get; set; }
        public string fla_numero { get; set; }
        public string fla_statusLegal { get; set; }
        public string fla_genero { get; set; }
        public string fla_donante { get; set; }
        public string fla_tipoSangre { get; set; }
        public string fla_talla { get; set; }
        public string fla_peso { get; set; }
        public string fla_tez { get; set; }
        public string fla_colorPelo { get; set; }
        public string fla_colorOjo { get; set; }
        public string fla_nombrePadre { get; set; }
        public string fla_nombreMadre { get; set; }
        public string fla_direccion { get; set; }
        public string fla_numeroDireccion { get; set; }
        public string fla_pueblo { get; set; }
        public string fla_codigoPostal { get; set; }
        public string fla_barrio { get; set; }
        public string fla_apartado { get; set; }
        public string fla_pueblo2 { get; set; }
        public string fla_codigoPostal2 { get; set; }
        public string fla_licencia { get; set; }
        public string fla_procedeLicencia { get; set; }
        public string fla_licenciaSuspendida { get; set; }
        public string fla_motivoSuspendido { get; set; }
        public string fla_recluido { get; set; }
        public string fla_convictoBebida { get; set; }
        public DateTime? fla_fechaConvictoBebida { get; set; }
        public string fla_convictoNarcotico { get; set; }
        public DateTime? fla_fechaConvictoNarcotico { get; set; }
        public string fla_obligacionAlimentaria { get; set; }
        public string fla_deudaAcca { get; set; }

    }
}
