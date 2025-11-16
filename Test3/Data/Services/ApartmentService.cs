using MongoDB.Bson;
using MongoDB.Driver;
using Test3.Data.Models;

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

        // Get apartments by landlord ID
        public async Task<List<Apartment>> GetApartmentsByLandlordIdAsync(string landlordId)
        {
            return await _apartments.Find(x => x.LandlordId == landlordId).ToListAsync();
        }

        // Search apartments by landlord with search term
        public async Task<List<Apartment>> SearchApartmentsByLandlordAsync(string landlordId, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return await GetApartmentsByLandlordIdAsync(landlordId);

            var filter = Builders<Apartment>.Filter.And(
                Builders<Apartment>.Filter.Eq(x => x.LandlordId, landlordId),
                Builders<Apartment>.Filter.Or(
                    Builders<Apartment>.Filter.Regex(x => x.ApartmentName,
                        new BsonRegularExpression(searchTerm, "i")),
                    Builders<Apartment>.Filter.Regex(x => x.Address,
                        new BsonRegularExpression(searchTerm, "i"))
                )
            );

            return await _apartments.Find(filter).ToListAsync();
        }

        // Create new apartment (empty document)
        public async Task CreateApartmentAsync(Apartment apartment)
        {
            await _apartments.InsertOneAsync(apartment);
        }

        // Delete apartment
        public async Task DeleteApartmentAsync(string id)
        {
            await _apartments.DeleteOneAsync(x => x.Id == id);
        }

        // Update apartment
        public async Task UpdateApartmentAsync(string id, Apartment apartment)
        {
            await _apartments.ReplaceOneAsync(x => x.Id == id, apartment);
        }
        // Add this to ApartmentService.cs
        public async Task AssignExistingApartmentsToLandlordsAsync()
        {
            // Get all landlords from the database
            var landlordsCollection = _apartments.Database.GetCollection<Landlord>("Landlord");
            var landlords = await landlordsCollection.Find(_ => true).ToListAsync();

            // Get all apartments that don't have a LandlordId
            var allApartments = await _apartments.Find(_ => true).ToListAsync();
            var apartmentsWithoutLandlord = allApartments.Where(a => string.IsNullOrEmpty(a.LandlordId)).ToList();

            Console.WriteLine($"Found {apartmentsWithoutLandlord.Count} apartments without landlord assignment");

            foreach (var apartment in apartmentsWithoutLandlord)
            {
                // Assign to the first landlord in the database
                var firstLandlord = landlords.FirstOrDefault();
                if (firstLandlord != null)
                {
                    apartment.LandlordId = firstLandlord.Id;
                    await _apartments.ReplaceOneAsync(x => x.Id == apartment.Id, apartment);
                    Console.WriteLine($"Assigned apartment '{apartment.ApartmentName}' to landlord '{firstLandlord.Username}'");
                }
            }

            Console.WriteLine("Finished assigning apartments to landlords");
        }
    }
}