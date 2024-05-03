using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace PastebinWebAPI.DAL.Models
{
    /// <summary>
    /// Модель таблицы "Posts".
    /// </summary>
    [Table("Posts")]
    public class Post : ModelBase
    {
        [Key]
        public override Guid Id { get; protected set; }

        /// <summary>
        /// Автор записи.
        /// </summary>
        [Required]
        public Guid AuthorId { get; set; }

        /// <summary>
        /// Текст поста. Хранится отдельно, чтобы в бедующим хранится текст поста в отдельной 
        /// </summary>
        [Required]
        public Guid TextId { get; set; }

        /// <summary>
        /// Стили текста.
        /// </summary>
        public string Metadata { get; set; } = string.Empty;
    }
}
