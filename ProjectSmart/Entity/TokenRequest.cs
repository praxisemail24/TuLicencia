namespace SmartLicencia.Entity
{
    public class TokenRequest
    {
        public string UserName { get; set; }
        public string ExpiredAt { get; set; }
        public int? UserId { get; set; }

        public TokenRequest()
        {
            UserName = string.Empty;
            ExpiredAt = string.Empty;
        }

        public DateTime GetExpiredAt()
        {
            if(string.IsNullOrWhiteSpace(ExpiredAt)) {
                return DateTime.Now.AddMinutes(30);
            } else {
                return Convert.ToDateTime(ExpiredAt);
            }
        }
    }
}
