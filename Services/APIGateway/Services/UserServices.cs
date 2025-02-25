using APIGateway.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway.Services
{
    public class UserServices
    {
        private const string QUEUE_AUTH_REQUESTS = "auth.requests";

        private readonly RabbitMqService _rabbitMq;

        public UserServices(RabbitMqService rabbitMq) 
        {
            _rabbitMq = rabbitMq;
        }

        private UserServices() { }

        public async Task<RegistrationResponse> Registration(RegistrationRequest request)
        {
            try
            {
                var result = await _rabbitMq.SendRequestToQueueAsync<RegistrationRequest, RegistrationResponse>(QUEUE_AUTH_REQUESTS,
                    request);

                return result ?? new RegistrationResponse();
            }
            catch (Exception ex)
            {
                return new RegistrationResponse();
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var result = await _rabbitMq.SendRequestToQueueAsync<LoginRequest, LoginResponse>(QUEUE_AUTH_REQUESTS,
                    request);
                return result ?? new LoginResponse();

            }
            catch (Exception ex)
            {

                return new LoginResponse();
            }
        }
    }
}
