using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TextService.Data.Models
{
    public class TextModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
