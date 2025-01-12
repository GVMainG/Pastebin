using HashService.Services;
using StackExchange.Redis;

namespace HashService.DAL
{
    public class RedisHashService
    {
        private readonly IDatabase _redisDb;

        public RedisHashService(string connectionString)
        {
            var redis = ConnectionMultiplexer.Connect(connectionString);
            _redisDb = redis.GetDatabase();
        }

        /// <summary>
        /// Сохраняет хэш в список Redis.
        /// </summary>
        /// <param name="hash">Хэш для сохранения.</param>
        public void SaveHash(string hash)
        {
            _redisDb.ListLeftPush("hashes", hash);
        }

        /// <summary>
        /// Получает хэш из Redis и удаляет его из списка.
        /// </summary>
        /// <returns>Хэш или null, если список пуст.</returns>
        public string GetHash()
        {
            // ListRightPop удаляет и возвращает последний элемент списка.
            return _redisDb.ListRightPop("hashes");
        }

        /// <summary>
        /// Предварительно загружает хэши в Redis.
        /// </summary>
        /// <param name="count">Количество хэшей для генерации.</param>
        /// <param name="generator">Экземпляр генератора хэшей.</param>
        public void PreloadHashes(int count, HashGeneratorService generator)
        {
            for (int i = 0; i < count; i++)
            {
                SaveHash(generator.GenerateHash());
            }
        }
    }
}
