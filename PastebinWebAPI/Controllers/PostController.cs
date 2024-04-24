using Microsoft.AspNetCore.Mvc;
using PastebinWebAPI.DAL;
using PastebinWebAPI.DAL.Models;

namespace PastebinWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        [HttpPost(Name = "AddPost")]
        public void Add([FromServices] UnitOfWorkBase postService, string text)
        {
            try
            {
                postService.Posts.Create(new Post() { Text = text });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
