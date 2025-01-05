namespace TextService.Models
{
    public class PopularityUpdateDto
    {
        public string TextId { get; set; }
        public int ViewCount { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
