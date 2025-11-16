using MongoDB.Driver;
using Test3.Data.Models;

namespace Test3.Data.Services
{
    public class LandlordService
    {
        private readonly IMongoCollection<Landlord> _landlords;

        public LandlordService(IMongoDatabase database)
        {
            _landlords = database.GetCollection<Landlord>("Landlord");
        }

        // Verify landlord credentials
        public async Task<Landlord?> LoginAsync(string username, string password)
        {
            try
            {
                var filter = Builders<Landlord>.Filter.And(
                    Builders<Landlord>.Filter.Eq(x => x.Username, username),
                    Builders<Landlord>.Filter.Eq(x => x.Password, password)
                );

                return await _landlords.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Login error: {ex.Message}");
                return null;
            }
        }

        // Get landlord by username (optional)
        public async Task<Landlord?> GetByUsernameAsync(string username)
        {
            return await _landlords.Find(x => x.Username == username).FirstOrDefaultAsync();
        }

        // Create new landlord (for initial setup)
        public async Task CreateLandlordAsync(string username, string password)
        {
            var landlord = new Landlord
            {
                Username = username,
                Password = password
            };

            await _landlords.InsertOneAsync(landlord);
        }

        // Check if any landlords exist (for initial setup)
        public async Task<bool> AnyLandlordsExistAsync()
        {
            return await _landlords.CountDocumentsAsync(_ => true) > 0;
        }
    }
}