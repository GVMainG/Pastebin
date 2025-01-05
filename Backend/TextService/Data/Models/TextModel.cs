﻿using MongoDB.Bson.Serialization.Attributes;

namespace TextService.Data.Models
{
    public class TextModel
    {
        [BsonId]
        public string Id { get; set; }
        public string Content { get; set; }
        public DateTime OnCreation { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
