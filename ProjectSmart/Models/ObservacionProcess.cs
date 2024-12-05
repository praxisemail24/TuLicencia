namespace SmartLicencia.Models
{
    public class ObservacionProcess
    {
        public Tramite tr_tramite { get; set; }
        public FormularioDTO frm_form { get; set; }
        public Administrador adm_admin { get; set; }
        public int ob_id { get; set; }        
        public string ob_comentario { get; set; }
        public int ob_estado { get; set; }
        public DateTime ob_fecha { get; set; }
        public Motivo mot_id { get; set; }

    }
}
