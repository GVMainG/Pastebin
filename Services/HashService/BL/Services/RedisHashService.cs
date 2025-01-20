using HashService.BL.Services.Interfaces;
using HashService.DAL;
using HashService.DAL.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace HashService.BL.Services
{
    public class RedisHashService
    {
        private readonly RedisHash _redisCache;
        private readonly IHashGeneratorService _hashGenerator;
        private readonly RabbitMqService _rabbitMqService;

        public RedisHashService(RedisHash redisCacheService, RabbitMqService rabbitMqService, IHashGeneratorService hashGenerator)
        {
            _redisCache = redisCacheService ?? throw new ArgumentNullException(nameof(redisCacheService));
            _rabbitMqService = rabbitMqService ?? throw new ArgumentNullException(nameof(rabbitMqService));
            _hashGenerator = hashGenerator ?? throw new ArgumentNullException(nameof(hashGenerator));
        }

        /// <summary>
        /// Запускает сервис и подписывается на запросы на хэши.
        /// </summary>
        public async Task Start()
        {
            try
            {
                await _rabbitMqService.RespondingToRequestsAsync<GetHashsRequest, HashModel>(HandleHashRequest);
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error starting RedisHashService: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Обрабатывает запрос на получение хэша.
        /// </summary>
        /// <param name="message">Сообщение запроса.</param>
        /// <returns>Модель хэша.</returns>
        private HashModel HandleHashRequest(GetHashsRequest message)
        {
            var hash = GetOrGenerateHash();
            _redisCache.SaveHash(hash);

            return new HashModel() { Hash = hash };
        }

        /// <summary>
        /// Получает хэш из Redis или генерирует новый, если он отсутствует.
        /// </summary>
        /// <returns>Хэш в виде строки.</returns>
        private string GetOrGenerateHash()
        {
            return _redisCache.GetHash() ?? _hashGenerator.GenerateHash();
        }
    }
}
