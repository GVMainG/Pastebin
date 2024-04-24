using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
