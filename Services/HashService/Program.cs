using HashService.DAL;
using HashService.Services;
using Pastebin.Infrastructure.SDK.Extensions;
using Pastebin.Infrastructure.SDK.Services;
using HS = HashService.BL.Services.RedisHashService;

namespace HashService
{
    public class Program
    {
        private static int i = 0;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient(x => {return new RedisHash(builder.Configuration.GetConnectionString("redis")); });
            builder.Services.AddSingleton(new RabbitMqService(builder.Configuration.GetConnectionString("rabbitmq")));
            builder.Services.AddSingleton<HS>();

            var app = builder.Build();

            app.Services.GetService<RedisHash>()?.PreloadHashes(100, new HashGeneratorService());
            app.Services.GetService<HS>()?.Start();

            app.Run();
        }
    }
}
