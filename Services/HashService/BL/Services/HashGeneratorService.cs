using System.Security.Cryptography;

namespace HashService.Services
{
    public class HashGeneratorService
    {
        public string GenerateHash()
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var buffer = new byte[8];
                rng.GetBytes(buffer);
                return Convert.ToBase64String(buffer)
                             .Replace("+", "")
                             .Replace("/", "")
                             .Replace("=", "")
                             .Substring(0, 8);
            }
        }
    }
}
