namespace SmartLicencia.Models
{
    public class Asignacion
    {
        public int asig_id { get; set; }
        public FormularioDTO frm_id { get; set; }
        public Tramite tr_id { get; set; }
        public Administrador adm_id1 { get; set; }
        public Administrador adm_id2 { get; set; }
        public DateTime asig_fecha { get; set; }
        public bool asig_Activo { get; set; }

        public string statusactual { get; set; }

        public string motivoAnulacion { get; set; }

        
    }
}
