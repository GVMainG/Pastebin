using HashService.DAL.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;
using PostService.BL.Models;
using PostService.BL.Services.Interfaces;
using PostService.DAL.Models;
using PostService.DAL.Repositories;

namespace PostService.BL.Services
{
    /// <summary>
    /// Сервис для работы с постами.
    /// </summary>
    public class PostService : IPostService
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly PostsMongoDbRepository _mongoRepository;
        private readonly PostsPostgreSQLRepository _postgresRepository;

        public PostService(RabbitMqService rabbitMqService, PostsMongoDbRepository mongoRepo, PostsPostgreSQLRepository postgresRepo)
        {
            _rabbitMqService = rabbitMqService ?? throw new ArgumentNullException(nameof(rabbitMqService));
            _mongoRepository = mongoRepo ?? throw new ArgumentNullException(nameof(mongoRepo));
            _postgresRepository = postgresRepo ?? throw new ArgumentNullException(nameof(postgresRepo));
        }

        /// <summary>
        /// Создание нового поста.
        /// </summary>
        /// <param name="text">Текст поста.</param>
        /// <returns>Хэш созданного поста.</returns>
        public async Task<string> CreatePostAsync(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Text cannot be null or empty.", nameof(text));

            var requestModel = new GetHashsRequest() { Count = 1 };

            var hash = await _rabbitMqService.SendRequestAsync<GetHashsRequest, HashModel>(requestModel);
            await _mongoRepository.SavePostAsync(new PostTextModel() 
            { 
                Hash = hash.Hash, 
                Text = text
            });
            await _postgresRepository.SaveMetadataAsync(new PostMetadataModel()
            {
                Hash = hash.Hash
            });

            return hash.Hash;
        }

        /// <summary>
        /// Получение поста по хэшу.
        /// </summary>
        /// <param name="hash">Хэш поста.</param>
        /// <returns>Ответ с данными поста.</returns>
        public async Task<PostResponse?> GetPostAsync(string hash)
        {
            if (string.IsNullOrEmpty(hash)) throw new ArgumentException("Hash cannot be null or empty.", nameof(hash));

            var text = await _mongoRepository.GetPostAsync(hash);
            var metadata = await _postgresRepository.GetMetadataAsync(hash);
            return text != null && metadata != null
                ? new PostResponse { Hash = hash, Text = text }
                : null;
        }
    }
}
