using HashService.DAL;
using HashService.DAL.Models;
using HashService.Services;
using Pastebin.Infrastructure.SDK.Services;

namespace HashService.BL.Services
{
    public class RedisHashService
    {
        private readonly DAL.RedisHashService _redisCache;
        private readonly HashGeneratorService _hashGenerator;
        private readonly RabbitMqService _rabbitMqService;

        public RedisHashService(DAL.RedisHashService redisCacheService, RabbitMqService rabbitMqService)
        {
            _redisCache = redisCacheService;
            _rabbitMqService = rabbitMqService;
            _hashGenerator = new HashGeneratorService();
        }

        public void Start()
        {
            // Подписка на запросы на хэши
            _rabbitMqService.SubscribeAsync<string>("hash_request", HandleHashRequest).Wait();
        }

        private void HandleHashRequest(string message)
        {
            var hash = _redisCache.GetHash() ?? _hashGenerator.GenerateHash();
            _redisCache.SaveHash(hash); // Пополнение Redis, если необходимо

            // Публикуем сгенерированный хэш
            _rabbitMqService.PublishMessage(new HashModel { Hash = hash }).Wait();
        }
    }
}
