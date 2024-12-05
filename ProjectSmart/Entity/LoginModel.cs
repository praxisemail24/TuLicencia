namespace SmartLicencia.Entity
{
    public class LoginModel
    {
        public string adm_user { get; set; }
        public string adm_clv { get; set; }
    }

    public class LoginModelCliente
    {
        public string cl_nombreUsuario { get; set; }
        public string cl_contrasena { get; set; }
    }

    public class LoginChangePasswordModel
    {
        public int Id { get; set; }
        public string? Password { get; set; }
        public string? RepeatPassword { get; set; }
    }
}
