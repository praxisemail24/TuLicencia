namespace WebApplication1.Models
{
    public class UserViewModel
    {
        public int adm_id { get; set; }
        public string? adm_user { get; set; }
        public string? adm_clv { get; set; }
        public string? adm_nombres { get; set; }
        public int adm_est { get; set; }
        public int adm_nivel { get; set; }
        public DateTime adm_fech_reg { get; set; }
        public string? adm_email { get; set; }
        public string? rol { get; set; }
        public string token { get; set; }

        public UserViewModel()
        {
            token = string.Empty;
        }
    }

    public class ResponseUser
    {
        public bool success { get; set; }
        public UserViewModel? item { get; set; }
    }
}
