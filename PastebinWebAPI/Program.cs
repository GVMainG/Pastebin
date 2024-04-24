using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using PastebinWebAPI.BLL.Infrastructure;
using PastebinWebAPI.DAL;



var builder = WebApplication.CreateBuilder(args);


// Add services to the container.


RegistrationModules();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

void RegistrationModules()
{
    var x = builder.Services.AddDbContext<PostgreSQLContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Mvc1Context")));

    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(builder =>
    {
        builder.RegisterModule(new PastebinWebAPIBLLModule());
    });
}
