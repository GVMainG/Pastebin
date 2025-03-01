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
            try
            {
                _logger.LogDebug($"{Registration}:" + "{@request}", request);

                var result = (await _rabbitMq.SendRequestToQueueAsync<RegistrationRequest, RegistrationResponse>(QUEUE_AUTH,
                    request)) ?? new RegistrationResponse();

                _logger.LogDebug($"{Registration}:" + "{@result}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Registration}:" + "{@request}", request);
                return new RegistrationResponse();
            }
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                _logger.LogDebug($"{Login}:" + "{@request}", request);

                var result = (await _rabbitMq.SendRequestToQueueAsync<LoginRequest, LoginResponse>(QUEUE_AUTH,
                    request)) ?? new LoginResponse();

                _logger.LogDebug($"{Login}:" + "{@result}", result);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{Login}:" + "{@request}", request);
                return new LoginResponse();
            }
        }

        public async Task UserEditRequest(UserEditRequest request)
        {
            try
            {
                _logger.LogDebug($"{UserEditRequest}:" + "{@request}", request);

                await _rabbitMq.PublishToQueueAsync(QUEUE_USERS_CREATE_DEL, request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{UserEditRequest}:" + "{@request}", request);
            }
        }

        public async Task UserDeleteRequest(Guid id)
        {
            try
            {
                _logger.LogDebug($"{UserDeleteRequest}:" + "{@id}", id);
                await _rabbitMq.PublishToQueueAsync(QUEUE_USERS_CREATE_DEL, new UserDeleteRequest(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"{UserDeleteRequest}:" + "{@id}", id);
            }
        }
    }
}
