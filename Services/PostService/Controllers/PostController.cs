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
            _postService = postService ?? throw new ArgumentNullException(nameof(postService));
        }

        /// <summary>
        /// Создание нового поста.
        /// </summary>
        /// <param name="request">Запрос на создание поста.</param>
        /// <returns>Ответ с хэшем созданного поста.</returns>
        [HttpPost("create")]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Text))
            {
                return BadRequest("Invalid request.");
            }

            var hash = await _postService.CreatePostAsync(request.Text);
            return Ok(new { Hash = hash });
        }

        /// <summary>
        /// Получение поста по хэшу.
        /// </summary>
        /// <param name="hash">Хэш поста.</param>
        /// <returns>Ответ с данными поста.</returns>
        [HttpGet("{hash}")]
        public async Task<IActionResult> GetPost(string hash)
        {
            if (string.IsNullOrEmpty(hash))
            {
                return BadRequest("Hash cannot be null or empty.");
            }

            var post = await _postService.GetPostAsync(hash);
            if (post == null)
                return NotFound();
            return Ok(post);
        }
    }
}
