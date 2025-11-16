using MongoDB.Bson;
using MongoDB.Driver;
using Test3.Models;

namespace Test3.Data.Services
{
    public class ApartmentService
    {
        private readonly IMongoCollection<Apartment> _apartments;

        public ApartmentService(IMongoDatabase database)
        {
            _apartments = database.GetCollection<Apartment>("Apartment");
        }

        // Get all apartments
        public async Task<List<Apartment>> GetApartmentsAsync()
        {
            return await _apartments.Find(_ => true).ToListAsync();
        }

        // Search apartments by name or address
        public async Task<List<Apartment>> SearchApartmentsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetApartmentsAsync();

            var filter = Builders<Apartment>.Filter.Or(
                Builders<Apartment>.Filter.Regex(x => x.ApartmentName,
                    new BsonRegularExpression(searchTerm, "i")),
                Builders<Apartment>.Filter.Regex(x => x.Address,
                    new BsonRegularExpression(searchTerm, "i"))
            );

            return await _apartments.Find(filter).ToListAsync();
        }
        public async Task<Apartment?> GetApartmentByIdAsync(string id)
        {
            try
            {
                Console.WriteLine($"Looking for apartment with ID: {id}");
                var apartment = await _apartments.Find(x => x.Id == id).FirstOrDefaultAsync();
                Console.WriteLine($"Found apartment: {apartment != null}");
                return apartment;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetApartmentByIdAsync: {ex.Message}");
                return null;
            }
        }
    }
}