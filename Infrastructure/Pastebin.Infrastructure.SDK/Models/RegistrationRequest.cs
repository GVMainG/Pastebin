using System.ComponentModel.DataAnnotations;

namespace APIGateway.Models
{
    public class RegistrationRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
