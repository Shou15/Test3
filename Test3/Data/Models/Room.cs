using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test3.Models
{
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonRepresentation(BsonType.ObjectId)]
        public string ApartmentId { get; set; } = string.Empty;

        [BsonElement("UnitNumber")]
        public string UnitNumber { get; set; } = string.Empty;

        [BsonElement("RoomType")]
        public string RoomType { get; set; } = string.Empty;

        [BsonElement("RoomPrice")]
        public decimal RoomPrice { get; set; }

        [BsonElement("Aircon")]
        public bool Aircon { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; } = "Available"; // Available, Occupied

        [BsonElement("Description")]
        public string Description { get; set; } = string.Empty;
    }
}