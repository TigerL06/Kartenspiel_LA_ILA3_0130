using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

public class Card
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("nummer")]
    public int? Nummer { get; set; } 

    [BsonElement("farbe")]
    public string Farbe { get; set; }

    [BsonElement("spezial")]
    public string Spezial { get; set; }
}
