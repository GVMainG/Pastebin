using Microsoft.AspNetCore.Mvc;
using PostService.Business.Models;
using PostService.Business.Services.Interfaces;

namespace PostService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;

        public PostsController(IPostService textService)
        {
            _postService = textService;
        }

        [HttpPost]
        public async Task<IActionResult> AddPost([FromBody] PostDTO postDTO)
        {
            var result = await _postService.AddPost(postDTO);
            return CreatedAtAction(nameof(AddPost), new { hash = result.Hash }, result);
        }

        [HttpPost("Hi")]
        public async Task<IActionResult> Get()
        {
            return Ok("Hi");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(string id)
        {
            var textBlock = await _postService.GetPost(id);
            if (textBlock == null)
                return NotFound();

            return Ok(textBlock);
        }
    }
}
