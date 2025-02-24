using APIGateway.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway.Services
{
    public class UserServices
    {
        private const string QUEUE_NAME = "authorization_and_registration";

        private readonly RabbitMqService _rabbitMq;

        public UserServices(RabbitMqService rabbitMq) 
        {
            _rabbitMq = rabbitMq;
        }

        private UserServices() { }

        public async Task<Guid> Registration(RegistrationRequest request)
        {
            try
            {
                var result = await _rabbitMq.SendRequestToQueueAsync<RegistrationRequest, RegistrationResponse>(QUEUE_NAME,
                    request);

                return result is not null && result.Id != Guid.Empty ?
                    result.Id : Guid.Empty;
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
        }

        public async Task<string> Login(LoginRequest request)
        {
            return string.Empty;
        }
    }
}
