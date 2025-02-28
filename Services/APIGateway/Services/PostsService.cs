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

        public PostsService(RabbitMqService rabbitMq)
        {
            _rabbitMq = rabbitMq;
        }

        public async Task<GetPostRespons> Get(string hash)
        {
            try
            {
                var result = await _rabbitMq.SendRequestToQueueAsync<GetPostRequest, GetPostRespons>(QUEUE_POSTS_GET, new GetPostRequest(hash));
                return result;
            }
            catch (Exception e)
            {
                return new GetPostRespons
                {
                    Hash = hash,
                    Content = string.Empty
                };
            }
        }

        public async Task<bool> CreateOrUpdate(CreateOrUpdatePostRequest request)
        {
            try
            {
                await _rabbitMq.PublishToQueueAsync(QUEUE_POSTS_CREATE_UPDATE, request);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> Delete(string hash)
        {
            try
            {
                await _rabbitMq.PublishToQueueAsync(QUEUE_POSTS_DELETE, new DeletePostRequest(hash));
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
