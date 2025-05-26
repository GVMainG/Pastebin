namespace Pastebin.Infrastructure.SDK.Models
{
    public class DeletePostRequest
    {
        public string Hash { get; set; }

        public DeletePostRequest(string hash)
        {
            Hash = hash;
        }

        private DeletePostRequest() { }
    }
}