using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace PostService.Data.Models
{
    public class PostModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("hash")]
        public string Hash { get; set; }

        [BsonElement("content")]
        public string Content { get; set; }

        [BsonElement("expirationDate")]
        public DateTime ExpirationDate { get; set; }

        [BsonElement("createdOn")]
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        [BsonElement("views")]
        public int Views { get; set; } = 0;
    }
}
