using APIGateway.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway.Services
{
    public class UserServices
    {
        private const string QUEUE_AUTH = "auth";
        private const string QUEUE_USERS_CREATE_DEL = "users.create_del";

        private readonly ILogger<UserServices> _logger;
        private readonly RabbitMqService _rabbitMq;

        public UserServices(RabbitMqService rabbitMq, ILogger<UserServices> logger)
        {
            if (rabbitMq == null)
                throw new ArgumentNullException(nameof(rabbitMq));
            if (logger == null)
                throw new ArgumentNullException(nameof(logger));

            _logger = logger;
            _rabbitMq = rabbitMq;
        }

        private UserServices() { }

        public async Task<RegistrationResponse> Registration(RegistrationRequest request)
        {

            var result = (await _rabbitMq.SendRequestToQueueAsync<RegistrationRequest, RegistrationResponse>(QUEUE_AUTH,
                request)) ?? new RegistrationResponse();
            return result;
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {

            var result = (await _rabbitMq.SendRequestToQueueAsync<LoginRequest, LoginResponse>(QUEUE_AUTH,
                request)) ?? new LoginResponse();
            return result;
        }

        public async Task UserEditRequest(UserEditRequest request)
        {
            await _rabbitMq.PublishToQueueAsync(QUEUE_USERS_CREATE_DEL, request);
        }

        public async Task UserDeleteRequest(Guid id)
        {
            await _rabbitMq.PublishToQueueAsync(QUEUE_USERS_CREATE_DEL, new UserDeleteRequest(id));
        }
    }
}
