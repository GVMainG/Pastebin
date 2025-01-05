using MongoDB.Driver;
using PostService.Business.Services.Interfaces;
using PostService.Data;
using PostService.Data.Repositorys.Interfaces;
using PostService.Data.Repositorys;
using PS = PostService.Business.Services.PostService;

namespace PostService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = InitializationWebApplicationBuilder(args);

            var app = builder.Build();

            //if (app.Environment.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }

        private static WebApplicationBuilder InitializationWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            InitializationServices(builder.Services, builder.Configuration);

            return builder;
        }

        private static void InitializationServices(IServiceCollection services, ConfigurationManager configuration)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Подключение MongoDB и регистрация MainMongoDbContext
            services.AddSingleton<IMongoClient>(provider =>
            {
                var settings = configuration.GetSection("MongoSettings");
                return new MongoClient(settings["ConnectionString"]);
            });

            services.AddSingleton<IMongoDatabase>(provider =>
            {
                var client = provider.GetRequiredService<IMongoClient>();
                var settings = configuration.GetSection("MongoSettings");
                return client.GetDatabase(settings["DatabaseName"]);
            });

            services.AddSingleton<MainMongoDbContext>();

            // Регистрация репозиториев. 
            services.AddScoped<IPostRepository, PostRepository>();

            services.AddHttpClient<IPostService, PS>(client =>
            {
                client.BaseAddress = new Uri("http://apigateway");
            });
        }
    }
}
