using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TextService.Models
{
    public class TextModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
