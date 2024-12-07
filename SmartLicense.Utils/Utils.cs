using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SmartLicense.Utils
{
    public class Utils
    {
        public static int Random(int min = 1, int max = 99999)
        {
            return RandomNumberGenerator.GetInt32(min, max + 1);
        }

        public static string RandomString(int length = 10)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                result.Append(chars[random.Next(chars.Length)]);

            return result.ToString();
        }

        public static string Slug(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // Convertir a minúsculas
            string slug = input.ToLowerInvariant();

            // Reemplazar caracteres acentuados o especiales
            slug = RemoveDiacritics(slug);

            // Reemplazar cualquier carácter que no sea letra, número o espacio por guiones
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Reemplazar múltiples espacios o guiones consecutivos con un solo guion
            slug = Regex.Replace(slug, @"[\s-]+", "-").Trim('-');

            return slug;
        }

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static async Task<string> CreateFile(string base64, string fileName, string dirBase)
        {
            var fileParts = base64.Split(",");

            if (fileParts.Length > 1)
            {
                base64 = fileParts[1];

                var mimeType = fileParts[0].Replace("data:", "").Replace(";base64", "");

                var extensions = new Dictionary<string, string>();
                extensions.Add("image/x-icon", "ico");
                extensions.Add("image/png", "png");
                extensions.Add("image/jpg", "jpg");
                extensions.Add("image/jpeg", "jpeg");
                extensions.Add("image/svg+xml", "svg");
                extensions.Add("text/plain", "txt");
                extensions.Add("text/css", "css");
                extensions.Add("text/csv", "csv");
                extensions.Add("text/html", "html");
                extensions.Add("application/pdf", "pdf");
                extensions.Add("application/javascript", "js");
                extensions.Add("application/json", "json");
                extensions.Add("application/java-archive", "jar");
                extensions.Add("application/x-sh", "sh");
                extensions.Add("application/zip", "zip");
                extensions.Add("application/x-7z-compressed", "7z");
                extensions.Add("application/xml", "xml");
                extensions.Add("application/vnd.ms-excel", "xls");
                extensions.Add("application/x-rar-compressed", "rar");
                extensions.Add("application/vnd.ms-powerpoint", "ppt");
                extensions.Add("application/msword", "doc");
                extensions.Add("video/x-msvideo", "avi");
                extensions.Add("audio/x-wav", "wav");

                fileName = $"{fileName}.{extensions[mimeType]}";
            }

            dirBase = Path.Combine(dirBase, fileName);

            if (!Directory.Exists(dirBase))
                Directory.CreateDirectory(dirBase);

            byte[] contents = Convert.FromBase64String(base64);

            await File.WriteAllBytesAsync(fileName, contents);

            return fileName;
        }
    }
}
