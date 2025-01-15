using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace PostService.DAL.Models
{
    public class PostTextModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Hash { get; set; }

        [BsonElement("Text")]
        public string Text { get; set; }
    }
}
