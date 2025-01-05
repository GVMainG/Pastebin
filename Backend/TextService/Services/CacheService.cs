using StackExchange.Redis;
using TextService.Models;
using TextService.Services.Interfaces;

namespace TextService.Services
{
    public class CacheService : ICacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private const string CachePrefix = "cache:text:";

        public CacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public async Task<bool> CacheTextAsync(string textId, string content)
        {
            var db = _redis.GetDatabase();
            return await db.StringSetAsync($"{CachePrefix}{textId}", content, TimeSpan.FromDays(7));
        }

        public async Task<string> GetCachedTextAsync(string textId)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync($"{CachePrefix}{textId}");
        }

        public async Task<IEnumerable<TextModel>> GetPopularTextsAsync()
        {
            var db = _redis.GetDatabase();
            var keys = await db.SetMembersAsync("popular_texts");

            var popularTexts = new List<TextModel>();
            foreach (var key in keys)
            {
                var content = await db.StringGetAsync($"{CachePrefix}{key}");
                if (!string.IsNullOrEmpty(content))
                {
                    popularTexts.Add(new TextModel { Id = key, Content = content });
                }
            }

            return popularTexts;
        }
    }
}
