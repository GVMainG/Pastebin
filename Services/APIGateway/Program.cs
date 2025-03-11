using APIGateway.Services;
using APIGateway.Tools;
using Pastebin.Infrastructure.SDK.Services;
using Prometheus;
using Serilog;
using Serilog.Context;

namespace APIGateway
{
    /// <summary>
    /// Главный класс программы
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Точка входа в приложение
        /// </summary>
        /// <param name="args">Аргументы командной строки</param>
        public static void Main(string[] args)
        {
            var builder = InitializationWebApplicationBuilder(args);
            var app = InitializationWebApplication(builder);

            app.Run();
        }

        /// <summary>
        /// Инициализация и конфигурация веб-приложения
        /// </summary>
        /// <param name="builder">Объект WebApplicationBuilder</param>
        /// <returns>Сконфигурированное веб-приложение</returns>
        private static WebApplication InitializationWebApplication(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // Включение логирования запросов Serilog.
            app.UseSerilogRequestLogging();

            // Конфигурация для режима разработки.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Включение аутентификации и авторизации.
            app.UseAuthentication();
            app.UseAuthorization();
            // Логирование HTTP-запросов.
            app.UseSerilogRequestLogging();

            // Middleware для добавления TraceId в лог.
            app.Use(async (context, next) =>
            {
                var traceId = context.TraceIdentifier;
                using (LogContext.PushProperty("TraceId", traceId))
                {
                    await next();
                }
            });

            // Middleware для логирования запросов и ответов.
            //app.Use(async (context, next) =>
            //{
            //    var request = context.Request;
            //    var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            //    Log.Information("HTTP Request {Method} {Path} {Query} {Body}", request.Method, request.Path, request.QueryString, requestBody);

            //    var originalResponseBodyStream = context.Response.Body;
            //    using var responseBodyStream = new MemoryStream();
            //    context.Response.Body = responseBodyStream;

            //    await next();

            //    context.Response.Body.Seek(0, SeekOrigin.Begin);
            //    var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
            //    context.Response.Body.Seek(0, SeekOrigin.Begin);

            //    Log.Information("HTTP Response {StatusCode} {Body}", context.Response.StatusCode, responseBody);
            //    await responseBodyStream.CopyToAsync(originalResponseBodyStream);
            //});

            app.UseMetricServer();
            // Считаем запросы, ошибки, время обработки.
            app.UseHttpMetrics();

            // Маршрутизация контроллеров.
            app.MapControllers();

            return app;
        }

        /// <summary>
        /// Инициализация и конфигурация WebApplicationBuilder.
        /// </summary>
        /// <param name="args">Аргументы командной строки.</param>
        /// <returns>Сконфигурированный WebApplicationBuilder.</returns>
        private static WebApplicationBuilder InitializationWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Конфигурация Serilog.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            // Инициализация сервисов
            InitializationServices(builder.Services, builder.Configuration);

            return builder;
        }

        /// <summary>
        /// Инициализация сервисов
        /// </summary>
        /// <param name="services">Коллекция сервисов</param>
        /// <param name="conf">Конфигурация приложения</param>
        private static void InitializationServices(IServiceCollection services, IConfiguration conf)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // Конфигурация RabbitMQ
            var rabbitMqConnectionString = conf.GetConnectionString("rabbitmq");
            if (string.IsNullOrEmpty(rabbitMqConnectionString))
                throw new InvalidOperationException("RabbitMQ connection string is not configured.");
            services.AddSingleton(new RabbitMqService(rabbitMqConnectionString));

            // Конфигурация JWT авторизации
            services.AddJWTAuthorization("Test");
            services.AddTransient<UserServices>();
            services.AddTransient<PostsService>();
        }
    }
}
