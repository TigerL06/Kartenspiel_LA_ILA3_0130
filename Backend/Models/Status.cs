using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
public class Status
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("anzahl")]
    public int Number { get; set; }


    [BsonElement("status")]
    public string status { get; set; }



}
