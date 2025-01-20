using System.ComponentModel.DataAnnotations;

namespace PostService.DAL.Models
{
    /// <summary>
    /// Модель метаданных поста.
    /// </summary>
    public class PostMetadataModel
    {
        /// <summary>
        /// Хэш поста, используется как ключ.
        /// </summary>
        [Key]
        public string Hash { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
