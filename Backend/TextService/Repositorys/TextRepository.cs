using MongoDB.Driver;
using TextService.Models;

namespace TextService.Repositorys
{
    public class TextRepository /*: ITextRepository*/
    {
        private readonly IMongoCollection<TextModel> _texts;

        public TextRepository(IMongoDatabase database)
        {
            _texts = database.GetCollection<TextModel>("texts");
        }

        public async Task<TextModel> CreateAsync(TextModel text)
        {
            await _texts.InsertOneAsync(text);
            return text;
        }

        public async Task<TextModel> GetByIdAsync(string id)
        {
            return await _texts.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var result = await _texts.DeleteOneAsync(x => x.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
