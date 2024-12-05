namespace WebApplication1.Models
{
    public class PDFViewModel
    {
        public string? Url { get; set; }
        public string? FileName { get; set; }
        public string? SignName { get; set; }
        public string SignUrl { get; set; }

        public bool Signed
        {
            get
            {
                return !string.IsNullOrWhiteSpace(SignName);
            }
        }

        public PDFViewModel()
        {
            SignUrl = "/pdf/sign";
        }
    }
}
