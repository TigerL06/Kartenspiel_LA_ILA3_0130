using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Backend.Models
{
    public class Card
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("nummer")]
        public int? Nummer { get; set; }

        [BsonElement("farbe")]
        public required string Farbe { get; set; }

        [BsonElement("spezial")]
        public required string Spezial { get; set; }
    }
}
