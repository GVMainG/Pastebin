using Microsoft.EntityFrameworkCore;
using Pastebin.Infrastructure.SDK.Extensions;
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

            builder.Services.AddRabbitMQService(builder.Configuration);

            builder.Services.AddDbContext<PostgreSQLContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
            builder.Services.AddTransient<MongoDbContext>();

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

            // Проверка и создание базы данных
            //using (var scope = app.Services.CreateScope())
            //{
            //    var dbContext = scope.ServiceProvider.GetRequiredService<PostgreSQLContext>();
            //    dbContext.Database.Migrate();
            //}

            app.Run();
        }
    }
}
