using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostService.DAL.Models
{
    /// <summary>
    /// Представляет модель данных для поста.
    /// </summary>
    [Table("Posts")]
    public class PostMetadataModel
    {
        /// <summary>
        /// Уникальный идентификатор поста.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        /// <summary>
        /// Уникальный хэш поста для генерации короткой ссылки.
        /// </summary>
        [Required]
        [MaxLength(8)]
        public string Hash { get; set; } = string.Empty;

        /// <summary>
        /// Дата и время создания поста.
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Дата и время последнего обновления поста.
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Дата и время, когда пост должен быть удалён.
        /// </summary>
        [Column(TypeName = "timestamp")]
        public DateTime? DeleteAt { get; set; }

        /// <summary>
        /// Флаг, указывающий, был ли пост проверен на необходимость удаления.
        /// </summary>
        [Required]
        public bool PassedDeletionCheck { get; set; } = false;

        /// <summary>
        /// Количество просмотров поста.
        /// </summary>
        [Required]
        public int Views { get; set; } = 0;
    }
}
