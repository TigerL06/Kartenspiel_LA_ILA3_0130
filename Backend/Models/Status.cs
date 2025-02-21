using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Backend.Models
{
    public class Status
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("anzahl")]
        public int Number { get; set; }

        [BsonElement("status")]
        public required string status { get; set; }
    }
}
