using MongoDB.Driver;
using TextService.Data;
using TextService.Data.Models;
using TextService.Data.Repositorys.Interfaces;

namespace TextService.Data.Repositorys
{
    public class TextRepository : ITextRepository
    {
        private readonly IMongoCollection<TextModel> _texts;

        public TextRepository(MongoDbContext context)
        {
            _texts = context.Texts;
        }

        // Создание текста
        public async Task<TextModel> CreateTextAsync(TextModel text)
        {
            await _texts.InsertOneAsync(text);
            return text;
        }

        // Получение текста по ID
        public async Task<TextModel> GetTextByIdAsync(string id)
        {
            return await _texts.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        // Удаление текста
        public async Task<bool> DeleteTextAsync(string id)
        {
            var result = await _texts.DeleteOneAsync(t => t.Id == id);
            return result.DeletedCount > 0;
        }

        // Получение всех текстов
        public async Task<IEnumerable<TextModel>> GetAllTextsAsync()
        {
            return await _texts.Find(_ => true).ToListAsync();
        }
    }
}
