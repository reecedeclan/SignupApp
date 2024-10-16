using Microsoft.Extensions.Options;
using MongoDB.Driver;
using SignupApp.Models;
using System.Threading.Tasks;

namespace SignupApp.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _users;
        private readonly MongoDbSettings _settings;

        public UserService(IOptions<MongoDbSettings> options)
        {
            _settings = options.Value;
            var mongoClient = new MongoClient(_settings.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(_settings.DatabaseName);
            _users = mongoDatabase.GetCollection<User>("Users");
        }

        // Method to register a new user
        public async Task<User> RegisterUserAsync(User user)
        {
            // Hash the password (implement your hashing logic here)
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);

            // Insert the user into the database
            await _users.InsertOneAsync(user);
            return user;
        }

        // Method to find a user by username
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _users.Find(user => user.Username == username).FirstOrDefaultAsync();
        }
    }
}
