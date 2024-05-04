using Autofac;
using Autofac.Extensions.DependencyInjection;
using PastebinWebAPI.BLL.Infrastructure;



var builder = WebApplication.CreateBuilder(args);

// Добавление модуля AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

ConnectingModelsAutoFac(builder);
ConnectingServices(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

static void ConnectingModelsAutoFac(WebApplicationBuilder builder)
{
    builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
    {
        containerBuilder.RegisterModule<PastebinWebAPIBLLModule>();
    });
}

static void ConnectingServices(WebApplicationBuilder builder)
{
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
}