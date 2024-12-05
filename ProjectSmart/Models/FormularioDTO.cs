namespace SmartLicencia.Models
{
    public class FormularioDTO
    {
        public Cliente cl_cliente { get; set; }
        public Pueblos cl_pueblos { get; set; }
        public Pago cl_pago { get; set; }
        public Tramite cl_tramite { get; set; }

        public string TipoTramite { get; set; }
        public string NombreTramite { get; set; }


        public int tr_id { get; set; }
        public int FrmID { get; set; }
        public DateTime? Fecha { get; set; }
        public int Estado { get; set; }
        public string TipoLicencia { get; set; }
        public string NumeroLicencia { get; set; }
        public string Categoria { get; set; }
        public string VehiculoPesado { get; set; }
        public string Identificacion { get; set; }
        public string Numero { get; set; }
        public string StatusLegal { get; set; }
        public string Genero { get; set; }
        public string Donante { get; set; }
        public string TipoSangre { get; set; }
        public string Talla { get; set; }
        public string Peso { get; set; }
        public string Tez { get; set; }
        public string ColorPelo { get; set; }
        public string ColorOjo { get; set; }
        public string Direccion { get; set; }
        public string NumeroDireccion { get; set; }
        public string Pueblo { get; set; }
        public string CodigoPostal { get; set; }
        public string Barrio { get; set; }
        public string Apartado { get; set; }
        public string Pueblo2 { get; set; }
        public string CodigoPostal2 { get; set; }
        public string LicenciaSuspendida { get; set; }
        public string MotivoSuspension { get; set; }
        public string Recluido { get; set; }
        public string ConvictoBebida { get; set; }
        public DateTime? FechaConvictoBebida { get; set; }
        public string ConvictoNarcotico { get; set; }
        public DateTime? FechaConvictoNarcotico { get; set; }
        public string ObligacionAlimentaria { get; set; }
        public string DeudaAcca { get; set; }
        public string ServiciosNecesarios { get; set; }


        public string TipoVehiculo { get; set; }
        public string PaisProcede { get; set; }
        public string EstadoProcede { get; set; }
        public string NumeroLicencia2 { get; set; }        
        public DateTime? FechaExpiracion { get; set; }
        public string NumeroIdentificacion { get; set; }
        public string NombrePadre { get; set; }
        public string NombreMadre { get; set; }
        public string NumeroLicenciaPR { get; set; }

        public int EstadoProceso { get; set; }
        public int EstadoRevision { get; set; }


        public decimal PorcAvance { get; set; }
        public string doctorAsignado { get; set; }
        public string estadoFormulario { get; set; }
        public string estadoMultas { get; set; }
        public string estadoEvaluacion { get; set; }
       
    
    }
}
