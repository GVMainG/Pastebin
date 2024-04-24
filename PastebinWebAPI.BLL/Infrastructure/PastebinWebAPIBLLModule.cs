using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PastebinWebAPI.DAL;



namespace PastebinWebAPI.BLL.Infrastructure
{
    public class PastebinWebAPIBLLModule() : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
            {
                var config = c.Resolve<IConfiguration>();
                return new EFUnitOfWork(config.GetSection("ConnectionStrings:db-pastebin-postgres:ConnectionString").Value);
            }).AsSelf().InstancePerLifetimeScope(); ;
        }
    }
}
