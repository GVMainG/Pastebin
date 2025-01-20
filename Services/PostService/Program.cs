using Microsoft.EntityFrameworkCore;
using Pastebin.Infrastructure.SDK.Services;
using PostService.BL.Services.Interfaces;
using PostService.DAL;
using PostService.DAL.Repositories;
using PS = PostService.BL.Services.PostService;

namespace PostService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Подключение RabbitMQ
            var rabbitMqConnectionString = builder.Configuration.GetConnectionString("rabbitmq");
            if (string.IsNullOrEmpty(rabbitMqConnectionString))
            {
                throw new InvalidOperationException("RabbitMQ connection string is not configured.");
            }
            builder.Services.AddSingleton(new RabbitMqService(rabbitMqConnectionString));

            // Подключение PostgreSQL
            var postgresConnectionString = builder.Configuration.GetConnectionString("postgres");
            if (string.IsNullOrEmpty(postgresConnectionString))
            {
                throw new InvalidOperationException("PostgreSQL connection string is not configured.");
            }
            builder.Services.AddDbContext<PostgreSQLContext>(options =>
                options.UseNpgsql(postgresConnectionString));

            // Подключение MongoDB
            var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb");
            if (string.IsNullOrEmpty(mongoConnectionString))
            {
                throw new InvalidOperationException("MongoDB connection string is not configured.");
            }
            builder.Services.AddSingleton(new MongoDbContext(mongoConnectionString));

            // Регистрация репозиториев
            builder.Services.AddScoped<PostsPostgreSQLRepository>();
            builder.Services.AddScoped<PostsMongoDbRepository>();

            builder.Services.AddScoped<IPostService, PS>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
