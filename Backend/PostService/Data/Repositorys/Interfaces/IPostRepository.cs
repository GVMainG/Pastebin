using PostService.Data.Models;

namespace PostService.Data.Repositorys.Interfaces
{
    public interface IPostRepository
    {
        Task<PostModel> Add(PostModel post);
        Task<PostModel> Get(string id);
        Task<bool> Delete(string id);
    }
}