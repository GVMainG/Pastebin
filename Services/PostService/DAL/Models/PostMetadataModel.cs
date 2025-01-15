using System.ComponentModel.DataAnnotations;

namespace PostService.DAL.Models
{
    public class PostMetadataModel
    {
        [Key]
        public string Hash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
