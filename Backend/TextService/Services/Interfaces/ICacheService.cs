using TextService.Models;

namespace TextService.Services.Interfaces
{
    public interface ICacheService
    {
        Task<bool> CacheTextAsync(string textId, string content);
        Task<string> GetCachedTextAsync(string textId);
        Task<IEnumerable<TextModel>> GetPopularTextsAsync();
    }
}
