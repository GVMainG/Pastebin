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
            builder.Services.AddSingleton(new RabbitMqService(builder.Configuration.GetConnectionString("rabbitmq")));

            builder.Services.AddDbContext<PostgreSQLContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("postgres")));
            // Подключение MongoDB
            builder.Services.AddSingleton(new MongoDbContext(builder.Configuration.GetConnectionString("mongodb")));

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
