namespace Pastebin.Infrastructure.SDK.Models
{
    public class UserDeleteRequest
    {
        private Guid Id { get; set; }

        public UserDeleteRequest(Guid id)
        {
            Id = id;
        }

        private UserDeleteRequest() { }
    }
}
