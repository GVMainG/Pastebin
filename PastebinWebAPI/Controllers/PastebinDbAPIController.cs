using Microsoft.AspNetCore.Mvc;



namespace PastebinWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PastebinDbAPIController : Controller
    {
        //[HttpPost(Name = "AddPost")]
        //public void Add([FromServices] PostService postService, string text)
        //{
        //    try
        //    {
        //        postService.Add(new Post() { Text = text });     
                
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //[HttpGet(Name = "GetPost")]
        //public IActionResult Get([FromServices] PostService postService, string text)
        //{
        //    try
        //    {
        //        postService.Get(x => x.Text == text);
        //        return View("Ok!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return View("Error");
        //    }
        //}
    }
}
