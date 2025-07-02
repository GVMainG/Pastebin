using APIGateway.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;
using System.Net.Http;
using System.Net.Http.Json;

namespace APIGateway.Services
{
    public class UserServices
    {
        private const string QUEUE_AUTH = "auth";
        private const string QUEUE_USERS_CREATE_DEL = "users.create_del";

        private readonly ILogger<UserServices> _logger;
        private readonly RabbitMqService _rabbitMq;
        private readonly HttpClient _httpClient;

        public UserServices(HttpClient httpClient, RabbitMqService rabbitMq, ILogger<UserServices> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
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

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);
            if (!response.IsSuccessStatusCode)
                return new RegistrationResponse();

            return await response.Content.ReadFromJsonAsync<RegistrationResponse>() ?? new RegistrationResponse();
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", request);
            if (!response.IsSuccessStatusCode)
                return new LoginResponse();

            return await response.Content.ReadFromJsonAsync<LoginResponse>() ?? new LoginResponse();
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
