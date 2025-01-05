using PostService.Business.Models;
using PostService.Business.Services.Interfaces;
using PostService.Data.Models;
using PostService.Data.Repositorys.Interfaces;

namespace PostService.Business.Services
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly HttpClient _httpClient;

        public PostService(IPostRepository postRepository, HttpClient httpClient)
        {
            _postRepository = postRepository;
            _httpClient = httpClient;
        }

        public async Task<PostDTO> AddPost(PostDTO postDTO)
        {
            if (postDTO.Content.Length > 10_485_760)
                throw new ArgumentException("Text exceeds the maximum allowed size.");

            // Запрос к API Gateway для получения хэша
            var hashResponse = await _httpClient.GetFromJsonAsync<HashResponse>("http://apigateway/hash");
            var hash = hashResponse?.Hash ?? throw new Exception("Failed to generate hash");

            var post = new PostModel
            {
                Hash = hash,
                Content = postDTO.Content,
                ExpirationDate = postDTO.ExpirationDate ?? DateTime.UtcNow.AddDays(7)
            };

            await _postRepository.Add(post);

            return postDTO;
        }

        public async Task<bool> DeletePost(string id)
        {
            return await _postRepository.Delete(id);
        }

        public async Task<PostDTO> GetPost(string id)
        {
            var post = await _postRepository.Get(id);

            return new PostDTO()
            {
                Id = id,
                Content = post.Content,
                ExpirationDate = post.ExpirationDate
            };
        }
    }
}
