using MassTransit;
using StackExchange.Redis;
using TextService.Data;
using TextService.Repositorys;
using TextService.Repositorys.Interfaces;
using TextService.Services;
using TextService.Services.Interfaces;
using TSS = TextService.Services.TextService;

var builderMain = WebApplicationBuilderInitialization();
var app = WebApplicationInitialization(builderMain);

app.Run();

#region ������

WebApplicationBuilder WebApplicationBuilderInitialization()
{
    var builder = WebApplication.CreateBuilder(args);

    RegistrationServices(builder.Services);

    return builder;
}

WebApplication WebApplicationInitialization(WebApplicationBuilder builder)
{
    var app = builder.Build();

    //// �������� ���� ������ � ��������� ��� ������
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MongoDbContext>();
        dbContext.EnsureCreated();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    return app;
}

void RegistrationServices(IServiceCollection services)
{
    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // ����������� MongoDbContext
    services.AddSingleton<MongoDbContext>();

    // Redis
    services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("172.18.0.1"));

    // ����������� � RabbitMQ � ��������� MassTransit
    services.AddMassTransit(x =>
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host("rabbitmq://172.18.0.1");
        });
    });

    services.AddScoped<ICacheService, CacheService>();
    services.AddScoped<ITextRepository, TextRepository>();

    // ����������� TextService � ������ ������������
    services.AddScoped<ITextService, TSS>();
}

#endregion ������