namespace SmartLicencia.Models
{
    public class CertifMed
    {
        public int fcm_id { get; set; }
        public Cliente cl_cliente { get; set; }
        public Pago pg_pago { get; set; }
        public Tramite tr_tramite { get; set; }       
        public int frm_id { get; set; }
        public string fcm_numeroSeguro { get; set; }
        public string fcm_numeroLicencia { get; set; }
        public string fcm_ojoDerechoSinLentes { get; set; }
        public string fcm_ojoDerechoConLentes { get; set; }
        public string fcm_ojoIzquierdoSinLentes { get; set; }
        public string fcm_ojoIzquierdoConLentes { get; set; }
        public string fcm_ambosOjos { get; set; }
        public string fcm_condicion { get; set; }
        public string fcm_espejuelos { get; set; }
        public string fcm_usaLentes { get; set; }
        public string fcm_condicionOido { get; set; }
        public string fcm_condicionBrazo { get; set; }
        public string fcm_condicionPierna { get; set; }
        public string fcm_condicionFisica { get; set; }
        public string fcm_observacion { get; set; }
        public string fcm_estadoInconciencia { get; set; }
        public string fcm_padeceCorazon { get; set; }
        public string fcm_marcapaso { get; set; }
        public string fcm_protesis { get; set; }
        public string fcm_estaturaPies { get; set; }
        public string fcm_estaturaPulgada { get; set; }
        public string fcm_peso { get; set; }
        public string fcm_colorPelo { get; set; }
        public string fcm_colorOjo { get; set; }
        public string fcm_nombreMedico { get; set; }
        public string fcm_licenciaMedico { get; set; }
        public DateTime fcm_fecha { get; set; }
        public int fcm_estado { get; set; }
    }
}
