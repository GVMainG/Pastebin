using MongoDB.Driver;
using PostService.DAL.Models;

namespace PostService.DAL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("PostServiceDb");
        }

        public IMongoCollection<PostTextModel> Posts => _database.GetCollection<PostTextModel>("Posts");
    }
}
