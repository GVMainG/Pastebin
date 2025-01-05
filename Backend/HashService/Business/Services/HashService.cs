using HashService.Business.Services.Interfaces;
using StackExchange.Redis;
using System.Security.Cryptography;
using System.Text;

namespace HashService.Business.Services
{
    public class HashService : IHashService
    {
        private const string AllowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private const int HashLength = 8;
        private readonly IDatabase _redisDb;
        private const string RedisKey = "available_hashes";

        public HashService(IConnectionMultiplexer redis)
        {
            _redisDb = redis.GetDatabase();
        }

        public async Task<string> GetHashAsync()
        {
            var hash = await _redisDb.ListLeftPopAsync(RedisKey);

            if (hash.IsNullOrEmpty)
            {
                // Генерация хэша, если Redis пуст.
                return GenerateSingleHash();
            }

            return hash.ToString();
        }

        public async Task GenerateHashesAsync(int count)
        {
            var hashes = new List<string>();

            for (int i = 0; i < count; i++)
            {
                hashes.Add(GenerateSingleHash());
            }

            await _redisDb.ListRightPushAsync(RedisKey, hashes.Select(x => (RedisValue)x).ToArray());
        }

        private string GenerateSingleHash()
        {
            var hash = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                var byteBuffer = new byte[HashLength];
                rng.GetBytes(byteBuffer);

                foreach (var @byte in byteBuffer)
                {
                    hash.Append(AllowedChars[@byte % AllowedChars.Length]);
                }
            }

            return hash.ToString();
        }
    }
}
