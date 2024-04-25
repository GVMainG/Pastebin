using Microsoft.AspNetCore.Mvc;
using PastebinWebAPI.BLL.Services.Interfaces;
using PastebinWebAPI.DAL;
using PastebinWebAPI.DAL.Models;

namespace PastebinWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly IPostService postService;

        public PostController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpPost(Name = "AddPost")]
        public void Add(string text)
        {
            try
            {
                postService.Add(new Post() { Text = text });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
