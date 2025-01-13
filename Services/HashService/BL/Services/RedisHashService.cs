using HashService.DAL;
using HashService.DAL.Models;
using HashService.Services;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace HashService.BL.Services
{
    public class RedisHashService
    {
        private readonly RedisHash _redisCache;
        private readonly HashGeneratorService _hashGenerator;
        private readonly RabbitMqService _rabbitMqService;

        public RedisHashService(RedisHash redisCacheService, RabbitMqService rabbitMqService)
        {
            _redisCache = redisCacheService;
            _rabbitMqService = rabbitMqService;
            _hashGenerator = new HashGeneratorService();
        }

        public async void Start()
        {
            // Подписка на запросы на хэши
            await _rabbitMqService.RespondingToRequests<GetHashsRequest, HashModel>(HandleHashRequest);
        }

        private HashModel HandleHashRequest(GetHashsRequest message)
        {
            var hash = _redisCache.GetHash() ?? _hashGenerator.GenerateHash();
            _redisCache.SaveHash(hash); // Пополнение Redis, если необходимо

            // Публикуем сгенерированный хэш
            return new HashModel() { Hash = hash };
        }
    }
}
