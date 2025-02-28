namespace Pastebin.Infrastructure.SDK.Models
{
    public class GetPostRequest
    {
        public string Hash { get; set; }

        public GetPostRequest(string hash)
        {
            Hash = hash;
        }

        private GetPostRequest() { }
    }
}
