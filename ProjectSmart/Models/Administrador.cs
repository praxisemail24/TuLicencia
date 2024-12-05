using System.ComponentModel.DataAnnotations.Schema;

namespace SmartLicencia.Models
{
    public class Administrador
    {
        public int adm_id { get; set; }
        public string adm_user { get; set; }
        public string adm_clv { get; set; }
        public string adm_nombres { get; set; }
        public int adm_est { get; set; }
        public int adm_nivel { get; set; }
        public DateTime adm_fech_reg { get; set; }
        public string adm_email { get; set; }

        public string? Token { get; set; }

        // Esta propiedad almacena la ruta del archivo en la base de datos.
        public string adm_firma { get; set; }

        // Esta propiedad se usa solo para recibir el archivo en la solicitud.
        [NotMapped]  // No se mapeará a la base de datos.
        public string FirmaBytes { get; set; }

        public string Rol
        {
            get
            {
                if (adm_nivel == 1)
                {
                    return "Administrador";
                }
                else if (adm_nivel == 2)
                {
                    return "Operador";
                }
                else if (adm_nivel == 3)
                {
                    return "Radicador";
                }
                else if (adm_nivel == 4)
                {
                    return "Doctor";
                }
                else
                {
                    return "Visitante";
                }
            }
        }
    }
}
