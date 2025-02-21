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

        [BsonElement("anzahl Spieler")]
        public int Number { get; set; }

        [BsonElement]
        public int CurrentUserNumber { get; set; }

        [BsonElement("status")]
        public required string status { get; set; }

        [BsonElement]
        public required string[] mixedCards { get; set; }

        [BsonElement]
        public required string[] laidCards { get; set; }
    }
}
