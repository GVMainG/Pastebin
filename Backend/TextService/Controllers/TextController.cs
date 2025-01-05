using Microsoft.AspNetCore.Mvc;
using TextService.Business.Models;
using TextService.Business.Services.Interfaces;

namespace TextService.Controllers
{
    [ApiController]
    [Route("api/texts")]
    public class TextController : ControllerBase
    {
        private readonly ITextService _textService;

        public TextController(ITextService textService)
        {
            _textService = textService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateText([FromBody] TextDto textDto)
        {
            var result = await _textService.CreateTextAsync(textDto);
            return CreatedAtAction(nameof(GetTextById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTextById(string id)
        {
            var text = await _textService.GetTextAsync(id);
            if (text == null) return NotFound();
            return Ok(text);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteText(string id)
        {
            var success = await _textService.DeleteTextAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
