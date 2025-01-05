using TextService.Data.Models;
using TextService.Models;

namespace TextService.Services.Interfaces
{
    public interface ITextService
    {
        Task<TextModel> CreateTextAsync(TextDto textDto);
        Task<TextModel> GetTextAsync(string id);
        Task<bool> DeleteTextAsync(string id);
        Task<IEnumerable<TextModel>> GetPopularTextsAsync();
    }
}
