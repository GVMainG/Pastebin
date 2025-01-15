using PostService.BL.Models;

namespace PostService.BL.Services.Interfaces
{
    public interface IPostService
    {
        Task<string> CreatePostAsync(string text);
        Task<PostResponse?> GetPostAsync(string hash);
    }
}
