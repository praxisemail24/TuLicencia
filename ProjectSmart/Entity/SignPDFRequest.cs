namespace SmartLicencia.Entity
{
    public class SignPDFRequest
    {
        public int Page { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public string Url { get; set; }
        public string FileName { get; set; }
        public string Sign { get; set; }

        private string _signName;

        public string SignName
        {
            get {
                if(string.IsNullOrWhiteSpace(_signName))
                {
                    return $"sign_{Guid.NewGuid().ToString()}";
                }
                return _signName;
            }
            set { _signName = value; }
        }


        public SignPDFRequest()
        {
            Url = string.Empty;
            FileName = string.Empty;
            Sign = string.Empty;
            Width = 150;
            Height = 85;
            _signName = string.Empty;
        }
    }
}
