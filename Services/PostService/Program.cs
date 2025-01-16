using Microsoft.EntityFrameworkCore;
using Pastebin.Infrastructure.SDK.Extensions;
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

            // ����������� ���������
            var isDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";

            // ������������ ����������� �����
            string postgresHost = isDocker ? "postgres" : "localhost";
            string mongoHost = isDocker ? "mongo" : "localhost";
            string rabbitHost = isDocker ? "rabbitmq" : "localhost";

            // ������������ ������ ����������� PostgreSQL
            var postgresConnection = $"Host={postgresHost};Port=5432;Database=postservice_db;Username=postgres;Password=postgres";

            // ������������ ������ ����������� MongoDB
            var mongoConnection = $"mongodb://{mongoHost}:27017";

            // ������������ ������ ����������� RabbitMQ
            var rabbitConnection = $"amqp://guest:guest@{rabbitHost}:5672/";



            // ����������� RabbitMQ
            builder.Services.AddSingleton(new RabbitMqService(rabbitConnection));

            builder.Services.AddDbContext<PostgreSQLContext>(options =>
                options.UseNpgsql(postgresConnection));
            // ����������� MongoDB
            builder.Services.AddSingleton(new MongoDbContext(mongoConnection));

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
