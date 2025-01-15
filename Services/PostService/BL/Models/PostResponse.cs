namespace PostService.BL.Models
{
    public class PostResponse
    {
        public string Hash { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
