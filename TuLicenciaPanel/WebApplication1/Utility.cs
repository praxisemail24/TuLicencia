using System.Security.Cryptography;
using System.Text;

namespace WebApplication1
{
    public class Utility
    {
        public static int Random(int min = 1, int max = 99999)
        {
            return RandomNumberGenerator.GetInt32(min, max + 1);
        }

        public static string RandomString(int length = 16)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            StringBuilder result = new StringBuilder(length);

            for (int i = 0; i < length; i++)
                result.Append(chars[random.Next(chars.Length)]);

            return result.ToString();
        }
    }
}
