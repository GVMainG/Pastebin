using Microsoft.AspNetCore.Mvc;
using PostService.Business.Models;
using PostService.Data.Models;

namespace PostService.Business.Services.Interfaces
{
    public interface IPostService
    {
        Task<PostDTO> GetPost(string id);
        Task<PostDTO> AddPost([FromBody] PostDTO post);
        Task<bool> DeletePost(string id);
    }
}