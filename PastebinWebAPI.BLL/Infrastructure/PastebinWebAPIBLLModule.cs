using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PastebinWebAPI.BLL.Services;
using PastebinWebAPI.BLL.Services.Interfaces;
using PastebinWebAPI.DAL;



namespace PastebinWebAPI.BLL.Infrastructure
{
    public class PastebinWebAPIBLLModule() : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Регистрация DbContext
            builder.Register(c => new PostgreSQLContext(
                    c.Resolve<IConfiguration>().GetConnectionString("db-pastebin-postgres")))
                .InstancePerLifetimeScope();

            // Регистрация сервиса
            builder.RegisterType<PostService>()
                .As<IPostService>()
                .InstancePerLifetimeScope();
        }
    }
}
