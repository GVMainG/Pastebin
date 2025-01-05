using TextService.Business.Models;
using TextService.Data.Models;

namespace TextService.Business.Services.Interfaces
{
    public interface ITextService
    {
        Task<TextModel> CreateTextAsync(TextDto textDto);
        Task<TextModel> GetTextAsync(string id);
        Task<bool> DeleteTextAsync(string id);
        Task<IEnumerable<TextModel>> GetPopularTextsAsync();
    }
}
