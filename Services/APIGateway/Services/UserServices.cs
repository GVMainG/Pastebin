using APIGateway.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway.Services
{
    public class UserServices
    {
        private const string QUEUE_AUTH = "auth";
        private const string SQUEUE_USERS_CREATE_DEL = "users.create_del";

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
                var result = await _rabbitMq.SendRequestToQueueAsync<RegistrationRequest, RegistrationResponse>(QUEUE_AUTH,
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
                var result = await _rabbitMq.SendRequestToQueueAsync<LoginRequest, LoginResponse>(QUEUE_AUTH,
                    request);
                return result ?? new LoginResponse();

            }
            catch (Exception ex)
            {

                return new LoginResponse();
            }
        }

        public async Task UserEditRequest(UserEditRequest request)
        {
            await _rabbitMq.PublishToQueueAsync<UserEditRequest>(SQUEUE_USERS_CREATE_DEL, request);
        }

        public async Task UserDeleteRequest(Guid id)
        {
            try
            {
                await _rabbitMq.PublishToQueueAsync<UserDeleteRequest>(SQUEUE_USERS_CREATE_DEL, new UserDeleteRequest(id));
            }
            catch (Exception ex)
            {
                // TODO: log.
            }
        }
    }
}
