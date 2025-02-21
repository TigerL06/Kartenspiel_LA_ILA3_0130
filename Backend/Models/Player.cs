using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Backend.Models
{
    public class Player
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("name")]
        public required string Name { get; set; }
    }
}
