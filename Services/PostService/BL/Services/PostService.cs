using HashService.DAL.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;
using PostService.BL.Models;
using PostService.BL.Services.Interfaces;
using PostService.DAL.Models;
using PostService.DAL.Repositories;

namespace PostService.BL.Services
{
    public class PostService : IPostService
    {
        private readonly RabbitMqService _rabbitMqService;
        private readonly PostsMongoDbRepository _mongoRepository;
        private readonly PostsPostgreSQLRepository _postgresRepository;

        public PostService(RabbitMqService rabbitMqService, PostsMongoDbRepository mongoRepo, PostsPostgreSQLRepository postgresRepo)
        {
            _rabbitMqService = rabbitMqService;
            _mongoRepository = mongoRepo;
            _postgresRepository = postgresRepo;
        }

        public async Task<string> CreatePostAsync(string text)
        {
            var requestModel = new GetHashsRequest() { Count = 1 };

            var hash = await _rabbitMqService.AsynchronousRequest<GetHashsRequest, HashModel>(requestModel);
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

        public async Task<PostResponse?> GetPostAsync(string hash)
        {
            var text = await _mongoRepository.GetPostAsync(hash);
            var metadata = await _postgresRepository.GetMetadataAsync(hash);
            return text != null && metadata != null
                ? new PostResponse { Hash = hash, Text = text, CreatedAt = metadata.CreatedAt }
                : null;
        }
    }
}
