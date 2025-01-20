using HashService.BL.Services.Interfaces;
using StackExchange.Redis;

namespace HashService.DAL
{
    public class RedisHash
    {
        private readonly IDatabase _redisDb;

        public RedisHash(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Строка подключения не может быть пустой.", nameof(connectionString));

            var redis = ConnectionMultiplexer.Connect(connectionString);
            _redisDb = redis.GetDatabase();
        }

        /// <summary>
        /// Сохраняет хэш в список Redis.
        /// </summary>
        /// <param name="hash">Хэш для сохранения.</param>
        public void SaveHash(string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentException("Хэш не может быть пустым.", nameof(hash));

            _redisDb.ListLeftPush("hashes", hash);
        }

        /// <summary>
        /// Получает хэш из Redis и удаляет его из списка.
        /// </summary>
        /// <returns>Хэш или null, если список пуст.</returns>
        public string GetHash()
        {
            return _redisDb.ListRightPop("hashes");
        }

        /// <summary>
        /// Предварительно загружает хэши в Redis.
        /// </summary>
        /// <param name="count">Количество хэшей для генерации.</param>
        /// <param name="generator">Экземпляр генератора хэшей.</param>
        public void PreloadHashes(int count, IHashGeneratorService generator)
        {
            if (count <= 0)
                throw new ArgumentException("Количество хэшей должно быть больше нуля.", nameof(count));

            if (generator == null)
                throw new ArgumentNullException(nameof(generator));

            for (int i = 0; i < count; i++)
            {
                SaveHash(generator.GenerateHash());
            }
        }
    }
}
