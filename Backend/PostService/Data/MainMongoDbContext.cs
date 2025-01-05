using MongoDB.Driver;
using PostService.Data.Models;

namespace PostService.Data
{
    public class MainMongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MainMongoDbContext(IMongoDatabase database)
        {
            _database = database;
        }

        public IMongoCollection<PostModel> TextBlocks => _database.GetCollection<PostModel>("Posts");
    }
}
