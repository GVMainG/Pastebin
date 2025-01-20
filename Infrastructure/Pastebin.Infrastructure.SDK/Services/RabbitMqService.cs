using EasyNetQ;

namespace Pastebin.Infrastructure.SDK.Services
{
    /// <summary>
    /// Сервис для работы с RabbitMQ, обеспечивающий функциональность публикации сообщений, подписки,
    /// обработки запросов и ответов.
    /// </summary>
    public class RabbitMqService : IDisposable
    {
        private readonly IBus _bus;

        /// <summary>
        /// Инициализирует сервис RabbitMQ.
        /// </summary>
        /// <param name="connectionString">Строка подключения к RabbitMQ.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если строка подключения пуста или null.</exception>
        public RabbitMqService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Строка подключения не может быть пустой.", nameof(connectionString));

            _bus = RabbitHutch.CreateBus(connectionString);
        }

        /// <summary>
        /// Публикует сообщение в RabbitMQ.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="message">Сообщение для публикации.</param>
        public async Task PublishMessageAsync<TMessage>(TMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await _bus.PubSub.PublishAsync(message);
        }

        /// <summary>
        /// Подписывается на получение сообщений указанного типа.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="name">Имя подписки.</param>
        /// <param name="messageHandler">Делегат для обработки полученных сообщений.</param>
        /// <exception cref="EasyNetQException">Выбрасывается, если задача не была выполнена.</exception>
        public async Task SubscribeAsync<TMessage>(string name, Action<TMessage> messageHandler)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Имя подписки не может быть пустым.", nameof(name));

            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            await _bus.PubSub.SubscribeAsync<TMessage>(name, async message =>
            {
                try
                {
                    await Task.Run(() => messageHandler(message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    throw new EasyNetQException("Message processing exception - look in the default error queue (broker)");
                }
            });
        }

        /// <summary>
        /// Отправляет асинхронный запрос и обрабатывает ответ.
        /// </summary>
        /// <typeparam name="TRequestMessage">Тип сообщения запроса.</typeparam>
        /// <typeparam name="TResponseMessage">Тип сообщения ответа.</typeparam>
        /// <param name="message">Сообщение запроса.</param>
        /// <param name="responseHandler">Делегат для обработки ответа.</param>
        public async Task<TResponseMessage> AsynchronousRequestAsync<TRequestMessage, TResponseMessage>(TRequestMessage message,
            Action<TResponseMessage> responseHandler = null)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            var response = await _bus.Rpc.RequestAsync<TRequestMessage, TResponseMessage>(message);
            responseHandler?.Invoke(response);

            return response;
        }

        /// <summary>
        /// Обрабатывает входящие запросы и возвращает ответы.
        /// </summary>
        /// <typeparam name="TRequestMessage">Тип входящего запроса.</typeparam>
        /// <typeparam name="TResponseMessage">Тип ответа.</typeparam>
        /// <param name="requestHandler">Функция для обработки запросов.</param>
        public async Task RespondingToRequestsAsync<TRequestMessage, TResponseMessage>(
            Func<TRequestMessage, TResponseMessage> requestHandler)
        {
            ArgumentNullException.ThrowIfNull(requestHandler);

            await _bus.Rpc.RespondAsync(requestHandler);
        }

        /// <summary>
        /// Закрытие подключения к RabbitMQ.
        /// </summary>
        public void Dispose() => _bus.Dispose();
    }
}
