using APIGateway.Services;
using APIGateway.Tools;
using Pastebin.Infrastructure.SDK.Services;
using Prometheus;
using Serilog;
using Serilog.Context;

namespace APIGateway
{
    /// <summary>
    /// ������� ����� ���������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ����� ����� � ����������
        /// </summary>
        /// <param name="args">��������� ��������� ������</param>
        public static void Main(string[] args)
        {
            var builder = InitializationWebApplicationBuilder(args);
            var app = InitializationWebApplication(builder);

            app.Run();
        }

        /// <summary>
        /// ������������� � ������������ ���-����������
        /// </summary>
        /// <param name="builder">������ WebApplicationBuilder</param>
        /// <returns>������������������ ���-����������</returns>
        private static WebApplication InitializationWebApplication(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // ��������� ����������� �������� Serilog.
            app.UseSerilogRequestLogging();

            // ������������ ��� ������ ����������.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // ��������� �������������� � �����������.
            app.UseAuthentication();
            app.UseAuthorization();
            // ����������� HTTP-��������.
            app.UseSerilogRequestLogging();

            // Middleware ��� ���������� TraceId � ���.
            app.Use(async (context, next) =>
            {
                var traceId = context.TraceIdentifier;
                using (LogContext.PushProperty("TraceId", traceId))
                {
                    await next();
                }
            });

            // Middleware ��� ����������� �������� � �������.
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
            // ������� �������, ������, ����� ���������.
            app.UseHttpMetrics();

            // ������������� ������������.
            app.MapControllers();

            return app;
        }

        /// <summary>
        /// ������������� � ������������ WebApplicationBuilder.
        /// </summary>
        /// <param name="args">��������� ��������� ������.</param>
        /// <returns>������������������ WebApplicationBuilder.</returns>
        private static WebApplicationBuilder InitializationWebApplicationBuilder(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ������������ Serilog.
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            builder.Host.UseSerilog();

            // ������������� ��������
            InitializationServices(builder.Services, builder.Configuration);

            return builder;
        }

        /// <summary>
        /// ������������� ��������
        /// </summary>
        /// <param name="services">��������� ��������</param>
        /// <param name="conf">������������ ����������</param>
        private static void InitializationServices(IServiceCollection services, IConfiguration conf)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            // ������������ RabbitMQ
            var rabbitMqConnectionString = conf.GetConnectionString("rabbitmq");
            if (string.IsNullOrEmpty(rabbitMqConnectionString))
                throw new InvalidOperationException("RabbitMQ connection string is not configured.");
            services.AddSingleton(new RabbitMqService(rabbitMqConnectionString));

            // ������������ JWT �����������
            services.AddJWTAuthorization("Test");
            services.AddTransient<UserServices>();
            services.AddTransient<PostsService>();
        }
    }
}
