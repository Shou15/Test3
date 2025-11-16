using MongoDB.Driver;
using Test3.Data.Models;

namespace Test3.Data.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(IMongoDatabase database, IConfiguration configuration)
        {
            var collectionName = configuration["MongoSettings:CollectionName"];
            _users = database.GetCollection<User>(collectionName);
        }

        public async Task CreateUser(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<User?> Login(string username, string password)
        {
            return await _users.Find(u => u.Username == username && u.Password == password)
                                .FirstOrDefaultAsync();
        }

        // Additional methods for tenant-like functionality
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            await _users.ReplaceOneAsync(u => u.Id == id, user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await _users.DeleteOneAsync(u => u.Id == id);
        }
    }
}