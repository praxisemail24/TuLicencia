namespace WebApplication1.Models
{
    public class CredentialViewModel
    {
        public string Usr { get; set; }
        public string Pwd { get; set; }
        public string? ReturnUrl { get; set; }

        public CredentialViewModel()
        {
            Usr = string.Empty;
            Pwd = string.Empty;
        }
    }
}
