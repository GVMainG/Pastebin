namespace APIGateway.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
        public Guid RoleId { get; set; }
    }
}