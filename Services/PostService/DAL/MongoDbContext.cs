using MongoDB.Driver;
using PostService.DAL.Models;

namespace PostService.DAL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException("Connection string cannot be null or empty.", nameof(connectionString));
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase("PostServiceDb");
        }

        public IMongoCollection<PostTextModel> Posts => _database.GetCollection<PostTextModel>("Posts");
    }
}
