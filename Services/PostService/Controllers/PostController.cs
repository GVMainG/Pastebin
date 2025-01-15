using Microsoft.AspNetCore.Mvc;
using PostService.BL.Models;
using PostService.BL.Services.Interfaces;

namespace PostService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            var hash = await _postService.CreatePostAsync(request.Text);
            return Ok(new { Hash = hash });
        }

        [HttpGet("{hash}")]
        public async Task<IActionResult> GetPost(string hash)
        {
            var post = await _postService.GetPostAsync(hash);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
    }
}
