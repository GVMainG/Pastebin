using MongoDB.Driver;
using TextService.Data.Models;



namespace TextService.Data
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;

        public MongoDbContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("MongoDbConnection"));
            _database = client.GetDatabase("PastebinDb");  // Название базы данных
        }

        // Коллекция текстов
        public IMongoCollection<TextModel> Texts => _database.GetCollection<TextModel>("texts");

        // Создание коллекции, если её нет
        public void EnsureCreated()
        {
            var collections = _database.ListCollectionNames().ToList();
            if (!collections.Contains("texts"))
            {
                _database.CreateCollection("texts");
                Console.WriteLine("Collection 'texts' created.");
            }
        }
    }
}
