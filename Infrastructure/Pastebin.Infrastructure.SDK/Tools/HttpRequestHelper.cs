using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Pastebin.Infrastructure.SDK.Tools
{
    /// <summary>
    /// Вспомогательный класс для выполнения HTTP-запросов с поддержкой JSON и JWT.
    /// </summary>
    public class HttpRequestHelper
    {
        private readonly HttpClient _httpClient;

        /// <summary>
        /// Инициализирует новый экземпляр класса HttpRequestHelper.
        /// </summary>
        /// <param name="httpClient">Экземпляр HttpClient, внедрённый через DI.</param>
        public HttpRequestHelper(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Устанавливает JWT-токен в заголовки авторизации.
        /// </summary>
        /// <param name="token">JWT-токен пользователя.</param>
        public void SetBearerToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// Выполняет GET-запрос и возвращает десериализованный объект.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
        /// <param name="url">URL-адрес запроса.</param>
        /// <returns>Ответ, преобразованный в указанный тип.</returns>
        public async Task<T?> GetAsync<T>(string url)
        {
            using var response = await _httpClient.GetAsync(url);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Выполняет POST-запрос с передачей данных и возвращает результат.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
        /// <param name="url">URL-адрес запроса.</param>
        /// <param name="data">Объект, сериализуемый в тело запроса.</param>
        /// <returns>Ответ, преобразованный в указанный тип.</returns>
        public async Task<T?> PostAsync<T>(string url, object data)
        {
            var content = SerializeContent(data);
            using var response = await _httpClient.PostAsync(url, content);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Выполняет PUT-запрос с передачей данных и возвращает результат.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
        /// <param name="url">URL-адрес запроса.</param>
        /// <param name="data">Объект, сериализуемый в тело запроса.</param>
        /// <returns>Ответ, преобразованный в указанный тип.</returns>
        public async Task<T?> PutAsync<T>(string url, object data)
        {
            var content = SerializeContent(data);
            using var response = await _httpClient.PutAsync(url, content);
            return await HandleResponse<T>(response);
        }

        /// <summary>
        /// Выполняет DELETE-запрос и возвращает true, если удаление прошло успешно.
        /// </summary>
        /// <param name="url">URL-адрес запроса.</param>
        /// <returns>Результат успешности выполнения запроса.</returns>
        public async Task<bool> DeleteAsync(string url)
        {
            using var response = await _httpClient.DeleteAsync(url);
            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// Сериализует объект в JSON-строку и упаковывает её в StringContent.
        /// </summary>
        /// <param name="data">Объект для сериализации.</param>
        /// <returns>Контент запроса в формате JSON.</returns>
        private static StringContent SerializeContent(object data)
        {
            var json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Обрабатывает HTTP-ответ: при успехе десериализует, иначе выбрасывает исключение.
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого объекта.</typeparam>
        /// <param name="response">HTTP-ответ от сервера.</param>
        /// <returns>Десериализованный объект или исключение.</returns>
        /// <exception cref="HttpRequestException">В случае ошибки HTTP.</exception>
        private static async Task<T?> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"HTTP error {response.StatusCode}: {errorContent}");
        }
    }
}
