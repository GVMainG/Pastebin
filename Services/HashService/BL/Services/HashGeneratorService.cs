using HashService.BL.Services.Interfaces;
using System.Security.Cryptography;

namespace HashService.BL.Services
{

    public class HashGeneratorService : IHashGeneratorService
    {
        private const int HashLength = 8;

        /// <summary>
        /// Генерирует случайный хеш.
        /// </summary>
        /// <returns>Случайный хеш в виде строки.</returns>
        public string GenerateHash()
        {
            var buffer = GenerateRandomBytes(HashLength);
            return ConvertToBase64String(buffer);
        }

        /// <summary>
        /// Генерирует массив случайных байтов заданной длины.
        /// </summary>
        /// <param name="length">Длина массива байтов.</param>
        /// <returns>Массив случайных байтов.</returns>
        private static byte[] GenerateRandomBytes(int length)
        {
            if (length <= 0)
                throw new ArgumentException("Длина массива байтов должна быть больше нуля.", nameof(length));

            using (var rng = RandomNumberGenerator.Create())
            {
                var buffer = new byte[length];
                rng.GetBytes(buffer);
                return buffer;
            }
        }

        /// <summary>
        /// Конвертирует массив байтов в строку Base64 и обрезает до нужной длины.
        /// </summary>
        /// <param name="buffer">Массив байтов.</param>
        /// <returns>Строка Base64.</returns>
        private static string ConvertToBase64String(byte[] buffer)
        {
            if (buffer == null || buffer.Length == 0)
                throw new ArgumentException("Массив байтов не может быть пустым.", nameof(buffer));

            return Convert.ToBase64String(buffer)
                          .Replace("+", "")
                          .Replace("/", "")
                          .Replace("=", "")
                          .Substring(0, HashLength);
        }
    }
}
