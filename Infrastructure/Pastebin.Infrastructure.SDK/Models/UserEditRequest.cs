namespace Pastebin.Infrastructure.SDK.Models
{
    public class UserEditRequest
    {
        public Guid Id { get; set; }
        public string Login { get; set; }
        public string Email { get; set; }
    }
}
