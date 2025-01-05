namespace PostService.Business.Models
{
    public class PostDTO
    {
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Content { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}