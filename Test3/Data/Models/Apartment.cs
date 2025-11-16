using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test3.Data.Models
{
    public class Apartment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("ApartmentName")]
        public string ApartmentName { get; set; } = string.Empty;

        [BsonElement("ApartmentOwner")]
        public string ApartmentOwner { get; set; } = string.Empty;

        [BsonElement("Address")]
        public string Address { get; set; } = string.Empty;

        [BsonElement("ContactNo")]
        public string ContactNo { get; set; } = string.Empty;

        [BsonElement("FacebookAccount")]
        public string FacebookAccount { get; set; } = string.Empty;

        [BsonElement("EmailAddress")]
        public string EmailAddress { get; set; } = string.Empty;

        // Optional: You can calculate these from rooms instead
        [BsonElement("TotalRooms")]
        public int TotalRooms { get; set; }

        [BsonElement("AvailableRooms")]
        public int AvailableRooms { get; set; }

        [BsonElement("LandlordId")]
        [BsonRepresentation(BsonType.ObjectId)]
        public string LandlordId { get; set; } = string.Empty;
        
        //Image
        [BsonElement("ImageData")]
        public byte[]? ImageData { get; set; }

        [BsonElement("ImageContentType")]
        public string ImageContentType { get; set; } = string.Empty;

        [BsonElement("ImageFileName")]
        public string ImageFileName { get; set; } = string.Empty;
    }
}