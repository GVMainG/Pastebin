using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway.Services
{
    public class PostsService
    {
        private const string QUEUE_POSTS_CREATE_UPDATE = "posts.create_update";
        private const string QUEUE_POSTS_DELETE = "posts.delete";
        private const string QUEUE_POSTS_GET = "posts.get";

        private readonly RabbitMqService _rabbitMq;
        private readonly ILogger<PostsService> _logger;

        public PostsService(RabbitMqService rabbitMq, ILogger<PostsService> logger)
        {
            if (rabbitMq == null)
                throw new ArgumentNullException(nameof(rabbitMq));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _rabbitMq = rabbitMq;
            _logger = logger;
        }

        public async Task<GetPostRespons> Get(string hash)
        {
            try
            {
                _logger.LogDebug($"{Get}:" + "{@hash}", hash);

                var result = await _rabbitMq.SendRequestToQueueAsync<GetPostRequest, GetPostRespons>(QUEUE_POSTS_GET, new GetPostRequest(hash));
                _logger.LogDebug($"{Get}:" + "{@result}", result);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{Get}:" + "{@hash}", hash);

                return new GetPostRespons
                {
                    Hash = hash,
                    Content = string.Empty
                };
            }
        }

        public async Task<bool> CreateOrUpdate(CreateOrUpdatePostRequest request)
        {
            await _rabbitMq.PublishToQueueAsync(QUEUE_POSTS_CREATE_UPDATE, request);
            return true;
        }

        public async Task<bool> Delete(string hash)
        {
            await _rabbitMq.PublishToQueueAsync(QUEUE_POSTS_DELETE, new DeletePostRequest(hash));
            return true;
        }
    }
}
