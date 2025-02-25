using APIGateway.Services;
using APIGateway.Tools;
using Pastebin.Infrastructure.SDK.Services;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var rabbitMqConnectionString = builder.Configuration.GetConnectionString("rabbitmq");
            if (string.IsNullOrEmpty(rabbitMqConnectionString))
            {
                throw new InvalidOperationException("RabbitMQ connection string is not configured.");
            }
            builder.Services.AddSingleton(new RabbitMqService(rabbitMqConnectionString));

            builder.Services.AddJWTAuthorization("Test");

            builder.Services.AddTransient<UserServices>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
