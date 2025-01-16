using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pastebin.Infrastructure.SDK.Services;

namespace Pastebin.Infrastructure.SDK.Extensions
{//
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует RabbitMQService в контейнере зависимостей.
        /// </summary>
        /// <param name="services">Сервисный контейнер.</param>
        /// <param name="configuration">Конфигурация приложения.</param>
        /// <returns>Обновленный сервисный контейнер.</returns>
        public static IServiceCollection AddRabbitMQService(this IServiceCollection services, IConfiguration conf)
        {
            services.AddTransient(_ => new RabbitMqService(conf.GetSection("RabbitMQSettings")["ConnectionString"]));
            return services;
        }
    }
}
