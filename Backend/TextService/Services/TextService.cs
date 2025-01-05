using MassTransit;
using MongoDB.Driver;
using TextService.Models;
using TextService.Services.Interfaces;

namespace TextService.Services
{
    /// <summary>
    /// Сервис по работе с текстовыми блокмаи.
    /// </summary>
    public class TextService : ITextService
    {
        private readonly IMongoCollection<TextModel> _texts;
        private readonly ICacheService _cacheService;
        private readonly IBus _bus;

        public TextService(IMongoDatabase database, ICacheService cacheService, IBus bus)
        {
            _texts = database.GetCollection<TextModel>("texts");
            _cacheService = cacheService;
            _bus = bus;
        }

        // Создание нового текста
        public async Task<TextModel> CreateTextAsync(TextDto textDto)
        {
            var newText = new TextModel
            {
                Content = textDto.Content,
                CreatedAt = DateTime.UtcNow,
                ExpirationDate = textDto.ExpirationDate ?? DateTime.UtcNow.AddDays(7) // по умолчанию 7 дней
            };

            await _texts.InsertOneAsync(newText);

            // Публикация события для кэширования
            await _bus.Publish(new PopularityUpdateDto
            {
                TextId = newText.Id
            });

            return newText;
        }

        // Получение текста по идентификатору
        public async Task<TextModel> GetTextAsync(string id)
        {
            // Сначала проверяем кэш
            var cachedText = await _cacheService.GetCachedTextAsync(id);
            if (!string.IsNullOrEmpty(cachedText))
            {
                Console.WriteLine($"[Cache Hit] Text {id} retrieved from cache.");
                return new TextModel { Id = id, Content = cachedText };
            }

            // Если в кэше нет, извлекаем из MongoDB
            var text = await _texts.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (text != null)
            {
                // Кэшируем текст, если он найден
                await _cacheService.CacheTextAsync(text.Id, text.Content);
            }

            return text;
        }

        // Удаление текста
        public async Task<bool> DeleteTextAsync(string id)
        {
            var result = await _texts.DeleteOneAsync(x => x.Id == id);
            if (result.DeletedCount > 0)
            {
                Console.WriteLine($"[TextService] Text {id} deleted from MongoDB.");
                return true;
            }
            return false;
        }

        // Получение популярных текстов
        public async Task<IEnumerable<TextModel>> GetPopularTextsAsync()
        {
            var texts = await _cacheService.GetPopularTextsAsync();
            return texts;
        }
    }
}
