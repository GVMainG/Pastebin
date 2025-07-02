using APIGateway.Models;
using Pastebin.Infrastructure.SDK.Models;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway.Services
{
    public class UserServices
    {
        private readonly ILogger<UserServices> _logger;
        private readonly RabbitMqService _rabbitMq;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        private IConfigurationSection _userServiceMethods;
        private string queueUsersEditDel;

        public UserServices(HttpClient httpClient, RabbitMqService rabbitMq, IConfiguration conf, ILogger<UserServices> logger)
        {
            _configuration = conf ?? throw new ArgumentNullException(nameof(conf));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _rabbitMq = rabbitMq ?? throw new ArgumentNullException(nameof(rabbitMq));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _userServiceMethods = _configuration
                .GetSection("Services")
                .GetSection("APIMethods")
                .GetSection("UserService");

            queueUsersEditDel = _configuration
                .GetSection("Services")
                .GetSection("Queues")
                .GetSection("QUEUE_USERS_EDIT_DEL")["name"] ?? string.Empty;
        }

        private UserServices() { }

        public async Task<RegistrationResponse> Registration(RegistrationRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_userServiceMethods["Registration"], request);

            if (!response.IsSuccessStatusCode)
                return new RegistrationResponse();

            return await response.Content.ReadFromJsonAsync<RegistrationResponse>() ?? new RegistrationResponse();
        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(_userServiceMethods["Login"], request);

            if (!response.IsSuccessStatusCode)
                return new LoginResponse();

            return await response.Content.ReadFromJsonAsync<LoginResponse>() ?? new LoginResponse();
        }

        public async Task UserEditRequest(UserEditRequest request)
        {
            await _rabbitMq.PublishToQueueAsync(queueUsersEditDel, request);
        }

        public async Task UserDeleteRequest(Guid id)
        {
            await _rabbitMq.PublishToQueueAsync(queueUsersEditDel, new UserDeleteRequest(id));
        }
    }
}
