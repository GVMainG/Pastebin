using HashService.DAL;
using HashService.Services;
using Pastebin.Infrastructure.SDK.Extensions;
using Pastebin.Infrastructure.SDK.Services;
using HS = HashService.BL.Services.RedisHashService;

namespace HashService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient(x => {return new RedisHashService("192.168.1.10:6379"); });
            builder.Services.AddRabbitMQService(builder.Configuration);
            builder.Services.AddSingleton<HS>();

            var app = builder.Build();

            app.Services.GetService<RedisHashService>()?.PreloadHashes(100, new HashGeneratorService());
            app.Services.GetService<HS>()?.Start();

            app.Services.GetService<RabbitMqService>().PublishMessage<string>("Hi! GV");

            app.Run();
        }
    }
}
