using APIGateway.Services;
using APIGateway.Tools;
using Pastebin.Infrastructure.SDK.Services;
using Serilog;

namespace APIGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = InitializationWebApplicationBuilder(args);
            var app = InitializationWebApplication(builder);

            app.Run();
        }

        private static WebApplication InitializationWebApplication(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        private static WebApplicationBuilder InitializationWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Настройка Serilog
            builder.Host.UseSerilog((context, config) =>
            {
                config
                    .Enrich.WithProperty("Service", "API Gateway") // Добавляет название сервиса в логи
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .WriteTo.File("logs/api-gateway.log", rollingInterval: RollingInterval.Day) // Ротация логов раз в день
                    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://elasticsearch:9200"))
                    {
                        AutoRegisterTemplate = true,
                        IndexFormat = "logs-api-gateway-{0:yyyy.MM.dd}"
                    });
            });

            InitializationServices(builder.Services, builder.Configuration);

            return builder;
        }

        private static void InitializationServices(IServiceCollection services, IConfiguration conf)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            var rabbitMqConnectionString = conf.GetConnectionString("rabbitmq");
            if (string.IsNullOrEmpty(rabbitMqConnectionString))
                throw new InvalidOperationException("RabbitMQ connection string is not configured.");
            services.AddSingleton(new RabbitMqService(rabbitMqConnectionString));

            services.AddJWTAuthorization("Test");
            services.AddTransient<UserServices>();
            services.AddTransient<PostsService>();
        }
    }
}
