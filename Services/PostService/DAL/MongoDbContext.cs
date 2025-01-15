using MongoDB.Driver;
using PostService.DAL.Models;

namespace PostService.DAL
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration conf)
        {
            var section = conf.GetSection("MongoDbSettings");

            var client = new MongoClient(section["ConnectionString"]);
            _database = client.GetDatabase(section["Database"]);
        }

        public IMongoCollection<PostTextModel> Posts => _database.GetCollection<PostTextModel>("Posts");
    }
}
