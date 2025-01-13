using HashService.DAL;
using HashService.DAL.Models;
using HashService.Services;
using Pastebin.Infrastructure.SDK.Extensions;
using Pastebin.Infrastructure.SDK.Models;
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

            builder.Services.AddTransient(x => {return new RedisHash("192.168.1.10:6379"); });
            builder.Services.AddRabbitMQService(builder.Configuration);
            builder.Services.AddSingleton<HS>();

            var app = builder.Build();

            app.Services.GetService<RedisHash>()?.PreloadHashes(100, new HashGeneratorService());
            app.Services.GetService<HS>()?.Start();

            app.Run();
        }

        private static void Test(HashModel request)
        {
            i++;
        }
    }
}
