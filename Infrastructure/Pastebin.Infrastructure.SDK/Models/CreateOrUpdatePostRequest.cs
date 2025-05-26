namespace Pastebin.Infrastructure.SDK.Models
{
    public class CreateOrUpdatePostRequest
    {
        public string Hash { get; set; }
        public string Content { get; set; }
    }
}