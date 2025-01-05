var builderMain = WebApplicationBuilderInitialization();
var app = WebApplicationInitialization(builderMain);

app.Run();

#region Ìועמהû

WebApplicationBuilder WebApplicationBuilderInitialization()
{
    var builder = WebApplication.CreateBuilder(args);

    RegistrationServices(builder.Services);

    return builder;
}

WebApplication WebApplicationInitialization(WebApplicationBuilder builder)
{
    var app = builder.Build();

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
}

#endregion Ìועמהû