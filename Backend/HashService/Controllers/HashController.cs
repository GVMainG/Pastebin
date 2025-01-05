using HashService.Business.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HashService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HashController : ControllerBase
    {
        private readonly IHashService _hashService;

        public HashController(IHashService hashService)
        {
            _hashService = hashService;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> GenerateHashes([FromQuery] int count = 100)
        {
            await _hashService.GenerateHashesAsync(count);
            return Ok($"Generated {count} hashes.");
        }

        [HttpGet]
        public async Task<IActionResult> GetHash()
        {
            var hash = await _hashService.GetHashAsync();
            return Ok(new { hash });
        }
    }
}
