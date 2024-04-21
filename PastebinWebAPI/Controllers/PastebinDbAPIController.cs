using Microsoft.AspNetCore.Mvc;
using PastebinWebAPI.DAL.Models;
using PastebinWebAPI.Services;



namespace PastebinWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PastebinDbAPIController : Controller
    {
        [HttpPost(Name = "AddPost")]
        public IActionResult Add([FromServices] PostService postService, string text)
        {
            try
            {
                postService.Add(new Post() { Text = text });
                return View("Ok!");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }

        [HttpGet(Name = "GetPost")]
        public IActionResult Get([FromServices] PostService postService, string text)
        {
            try
            {
                postService.Get(x => x.Text == text);
                return View("Ok!");
            }
            catch (Exception ex)
            {
                return View("Error");
            }
        }
    }
}
