using HashService.BL.Services;
using HashService.BL.Services.Interfaces;
using HashService.DAL;
using Pastebin.Infrastructure.SDK.Services;
using HS = HashService.BL.Services.RedisHashService;

namespace HashService
{
    public class Program
    {
        public static void  Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            InitializeServices(app);

            app.Run();
        }

        private static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient(x => new RedisHash(builder.Configuration.GetConnectionString("redis")));
            builder.Services.AddSingleton(new RabbitMqService(builder.Configuration.GetConnectionString("rabbitmq")));
            builder.Services.AddSingleton<IHashGeneratorService, HashGeneratorService>();
            builder.Services.AddSingleton<HS>();
        }

        private static async void InitializeServices(WebApplication app)
        {
            try
            {
                var redisHash = app.Services.GetService<RedisHash>();
                redisHash?.PreloadHashes(100, new HashGeneratorService());

                var redisHashService = app.Services.GetService<HS>();
                if (redisHashService != null)
                {
                    await redisHashService.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing services: {ex.Message}");
                throw;
            }
        }
    }
}
