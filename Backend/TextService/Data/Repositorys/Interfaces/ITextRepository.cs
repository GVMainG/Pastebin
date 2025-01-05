using TextService.Data.Models;

namespace TextService.Data.Repositorys.Interfaces
{
    public interface ITextRepository
    {
        Task<TextModel> CreateTextAsync(TextModel text);
        Task<TextModel> GetTextByIdAsync(string id);
        Task<bool> DeleteTextAsync(string id);
        Task<IEnumerable<TextModel>> GetAllTextsAsync();
    }
}
