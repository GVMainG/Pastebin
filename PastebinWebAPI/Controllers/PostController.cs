using Microsoft.AspNetCore.Mvc;
using PastebinWebAPI.BLL.Services.Interfaces;
using PastebinWebAPI.DAL.Models;



namespace PastebinWebAPI.Controllers
{
    /// <summary>
    /// КОнтролер по работе с постами.
    /// </summary>
    /// <param name="postService"></param>
    [ApiController]
    [Route("[controller]")]
    public class PostController(IPostService postService) : Controller
    {
        private readonly IPostService postService = postService;

        /// <summary>
        /// Создание поста.
        /// </summary>
        /// <param name="text">Текст поста.</param>
        [HttpPost(Name = "CreatePost")]
        public void Create(string text)
        {
            postService.Create(new Post());
        }
    }
}
