using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Program
    {
        public async static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Добавление конфигурации Ocelot
            builder.Configuration.AddJsonFile("ocelot.json");
            builder.Services.AddOcelot();

            var app = builder.Build();

            await app.UseOcelot();

            app.Run();
        }
    }
}
