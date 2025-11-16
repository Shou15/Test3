using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Test3.Data.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; } = string.Empty;

        public string Username { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Birthdate { get; set; }

        public string Age { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;
    }
}