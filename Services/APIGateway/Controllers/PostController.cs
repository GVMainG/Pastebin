using APIGateway.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pastebin.Infrastructure.SDK.Models;

namespace APIGateway.Controllers
{
    [Route("posts")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private PostsService _postsService;
        private ILogger<PostController> _logger;

        public PostController(PostsService postsService, ILogger<PostController> logger)
        {
            _postsService = postsService;
            _logger = logger;
        }

        [HttpGet("{hash}")]
        public async Task<IActionResult> Get(string hash)
        {
            _logger.LogDebug($"{nameof(Get)}:" + "{@hash}", hash);

            var result = await _postsService.Get(hash);
            if (result == null || string.IsNullOrEmpty(result.Content))
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdatePostRequest request)
        {
            _logger.LogDebug($"{nameof(CreateOrUpdate)}:" + "{@request}", request);

            var result = await _postsService.CreateOrUpdate(request);
            return result ? Ok() : BadRequest();
        }

        [HttpDelete("del/{hash}")]
        public async Task<IActionResult> Delete(string hash)
        {
            _logger.LogDebug($"{nameof(Delete)}:" + "{@hash}", hash);

            var result = await _postsService.Delete(hash);
            return result ? Ok() : BadRequest();
        }
    }
}
