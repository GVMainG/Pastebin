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
        /// <exception cref="ArgumentNullException">Выбрасывается, если сообщение null.</exception>
        public async Task PublishAsync<TMessage>(TMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await _bus.PubSub.PublishAsync(message);
        }

        /// <summary>
        /// Публикует сообщение в указанную очередь.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="message">Сообщение для публикации.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если имя очереди пустое или null.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сообщение null.</exception>
        public async Task PublishToQueueAsync<TMessage>(string queueName, TMessage message)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Имя очереди не может быть пустым.", nameof(queueName));

            if (message == null)
                throw new ArgumentNullException(nameof(message));

            await _bus.Advanced.QueueDeclareAsync(queueName);
            await _bus.SendReceive.SendAsync(queueName, message);
        }

        /// <summary>
        /// Подписывается на получение сообщений указанного типа.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="subscriptionId">Идентификатор подписки.</param>
        /// <param name="messageHandler">Делегат для обработки полученных сообщений.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если идентификатор подписки пустой или null.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если делегат обработки сообщений null.</exception>
        /// <exception cref="EasyNetQException">Выбрасывается, если задача не была выполнена.</exception>
        public async Task SubscribeAsync<TMessage>(Guid subscriptionId, Action<TMessage> messageHandler)
        {
            if (string.IsNullOrWhiteSpace(subscriptionId.ToString()))
                throw new ArgumentException("Идентификатор подписки не может быть пустым.", nameof(subscriptionId));

            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            await _bus.PubSub.SubscribeAsync<TMessage>(subscriptionId.ToString(), async message =>
            {
                try
                {
                    await Task.Run(() => messageHandler(message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
                    throw new EasyNetQException("Исключение обработки сообщения - смотрите в очереди ошибок по умолчанию (broker)");
                }
            });
        }

        /// <summary>
        /// Подписывается на получение сообщений указанного типа из указанной очереди.
        /// </summary>
        /// <typeparam name="TMessage">Тип сообщения.</typeparam>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="messageHandler">Делегат для обработки полученных сообщений.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если имя очереди пустое или null.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если делегат обработки сообщений null.</exception>
        /// <exception cref="EasyNetQException">Выбрасывается, если задача не была выполнена.</exception>
        public async Task SubscribeToQueueAsync<TMessage>(string queueName, Action<TMessage> messageHandler)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Имя очереди не может быть пустым.", nameof(queueName));

            if (messageHandler == null)
                throw new ArgumentNullException(nameof(messageHandler));

            await _bus.Advanced.QueueDeclareAsync(queueName);
            await _bus.SendReceive.ReceiveAsync<TMessage>(queueName, async message =>
            {
                try
                {
                    await Task.Run(() => messageHandler(message));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка обработки сообщения: {ex.Message}");
                    throw new EasyNetQException("Исключение обработки сообщения - смотрите в очереди ошибок по умолчанию (broker)");
                }
            });
        }

        /// <summary>
        /// Отправляет асинхронный запрос и обрабатывает ответ.
        /// </summary>
        /// <typeparam name="TRequest">Тип сообщения запроса.</typeparam>
        /// <typeparam name="TResponse">Тип сообщения ответа.</typeparam>
        /// <param name="request">Сообщение запроса.</param>
        /// <param name="responseHandler">Делегат для обработки ответа.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сообщение запроса null.</exception>
        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request,
            Action<TResponse> responseHandler = null)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var response = await _bus.Rpc.RequestAsync<TRequest, TResponse>(request);
            responseHandler?.Invoke(response);

            return response;
        }

        public async Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var response = await _bus.Rpc.RequestAsync<TRequest, TResponse>(request);
            return response;
        }

        /// <summary>
        /// Отправляет асинхронный запрос в указанную очередь и обрабатывает ответ.
        /// </summary>
        /// <typeparam name="TRequest">Тип сообщения запроса.</typeparam>
        /// <typeparam name="TResponse">Тип сообщения ответа.</typeparam>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="request">Сообщение запроса.</param>
        /// <param name="responseHandler">Делегат для обработки ответа.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если имя очереди пустое или null.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если сообщение запроса null.</exception>
        public async Task<TResponse> SendRequestToQueueAsync<TRequest, TResponse>(string queueName, TRequest request,
            Action<TResponse> responseHandler = null)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Имя очереди не может быть пустым.", nameof(queueName));

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            await _bus.Advanced.QueueDeclareAsync(queueName);
            var response = await _bus.Rpc.RequestAsync<TRequest, TResponse>(request, config => config.WithQueueName(queueName));
            responseHandler?.Invoke(response);

            return response;
        }

        /// <summary>
        /// Обрабатывает входящие запросы и возвращает ответы.
        /// </summary>
        /// <typeparam name="TRequest">Тип входящего запроса.</typeparam>
        /// <typeparam name="TResponse">Тип ответа.</typeparam>
        /// <param name="requestHandler">Функция для обработки запросов.</param>
        /// <exception cref="ArgumentNullException">Выбрасывается, если функция обработки запросов null.</exception>
        public async Task HandleRequestsAsync<TRequest, TResponse>(
            Func<TRequest, TResponse> requestHandler)
        {
            ArgumentNullException.ThrowIfNull(requestHandler);

            await _bus.Rpc.RespondAsync<TRequest, TResponse>(requestHandler);
        }

        /// <summary>
        /// Обрабатывает входящие запросы из указанной очереди и возвращает ответы.
        /// </summary>
        /// <typeparam name="TRequest">Тип входящего запроса.</typeparam>
        /// <typeparam name="TResponse">Тип ответа.</typeparam>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="requestHandler">Функция для обработки запросов.</param>
        /// <exception cref="ArgumentException">Выбрасывается, если имя очереди пустое или null.</exception>
        /// <exception cref="ArgumentNullException">Выбрасывается, если функция обработки запросов null.</exception>
        public async Task HandleRequestsFromQueueAsync<TRequest, TResponse>(string queueName,
            Func<TRequest, CancellationToken, Task<TResponse>> requestHandler)
        {
            if (string.IsNullOrWhiteSpace(queueName))
                throw new ArgumentException("Имя очереди не может быть пустым.", nameof(queueName));

            ArgumentNullException.ThrowIfNull(requestHandler);

            await _bus.Advanced.QueueDeclareAsync(queueName);
            await _bus.Rpc.RespondAsync(requestHandler, config => config.WithQueueName(queueName));
        }

        /// <summary>
        /// Закрытие подключения к RabbitMQ.
        /// </summary>
        public void Dispose() => _bus.Dispose();
    }
}
