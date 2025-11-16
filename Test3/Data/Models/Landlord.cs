using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test3.Data.Models
{
    public class Landlord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("Username")]
        public string Username { get; set; } = string.Empty;

        [BsonElement("Password")]
        public string Password { get; set; } = string.Empty;
    }
}