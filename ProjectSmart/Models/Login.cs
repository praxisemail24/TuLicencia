namespace SmartLicencia.Models
{
    public class Login
    {

        public Cliente Cliente { get; set; }

        public int lg_id { get; set; }
        public string lg_usuario { get; set; }
        public string lg_constraseña { get; set; }

    }
}
