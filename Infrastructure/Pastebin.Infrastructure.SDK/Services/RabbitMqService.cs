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

            _bus = RabbitHutch.CreateBus(connectionString); // Инициализация подключения к RabbitMQ.
        }

        /// <summary>
        /// Публикует сообщение в RabbitMQ.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="message">Сообщение для публикации.</param>
        public async Task PublishMessage<TMessage>(TMessage message)
        {
            await _bus.PubSub.PublishAsync(message); // Асинхронная публикация сообщения.
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
            await _bus.PubSub.SubscribeAsync<TMessage>(name,
            message => Task.Factory.StartNew(() =>
            {
                messageHandler(message); // Обработка сообщения.
            }).ContinueWith(task =>
            {
                if (task.IsCompleted && !task.IsFaulted)
                {
                    // TODO: Добавить обработчик выполненных задач.
                }
                else
                {
                    // TODO: Добавить логирование или обработчик ошибок, если задача не выполнена.
                    throw new EasyNetQException("Message processing exception - " +
                        "look in the default error queue (broker)");
                }
            }));
        }

        /// <summary>
        /// Отправляет асинхронный запрос и обрабатывает ответ.
        /// </summary>
        /// <typeparam name="TRequestMessage">Тип сообщения запроса.</typeparam>
        /// <typeparam name="TResponseMessage">Тип сообщения ответа.</typeparam>
        /// <param name="message">Сообщение запроса.</param>
        /// <param name="responseHandler">Делегат для обработки ответа.</param>
        public async Task AsynchronousRequest<TRequestMessage, TResponseMessage>(TRequestMessage message,
            Action<TResponseMessage> responseHandler)
        {
            var task = _bus.Rpc.RequestAsync<TRequestMessage, TResponseMessage>(message);
            await task.ContinueWith(async response =>
            {
                responseHandler(await response); // Обработка ответа.
            });
        }

        /// <summary>
        /// Обрабатывает входящие запросы и возвращает ответы.
        /// </summary>
        /// <typeparam name="TRequestMessage">Тип входящего запроса.</typeparam>
        /// <typeparam name="TResponseMessage">Тип ответа.</typeparam>
        /// <param name="message">Сообщение ответа.</param>
        /// <param name="requestHandler">Функция для обработки запросов.</param>
        public async Task RespondingToRequests<TRequestMessage, TResponseMessage>(
            Func<TRequestMessage, TResponseMessage> requestHandler)
        {
            await _bus.Rpc.RespondAsync<TRequestMessage, TResponseMessage>(request => requestHandler(request));
        }

        /// <summary>
        /// Закрытие подключения к RabbitMQ.
        /// </summary>
        public void Dispose() => _bus.Dispose(); 
    }
}
