namespace SmartLicencia.Models
{
    public class Token
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string AccessToken { get; set; }
        public DateTime ExpiredAt { get; set; }
        public DateTime? RevokedAt { get; set; }
        public int? UserId { get; set; }
        public int UseOrigin { get; set; }

        public Token()
        {
            UserName = string.Empty;
            AccessToken = string.Empty;
            UseOrigin = 0;
        }
    }
}
