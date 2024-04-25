using System.ComponentModel.DataAnnotations;



namespace PastebinWebAPI.DAL.Models
{
    public class Post : ModelBase
    {
        [Key]
        public override Guid Id { get; protected set; }

        [Required]
        [MaxLength(10)]
        public string Text { get; set; } = string.Empty;
    }
}
