using QuizPortal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizPortal.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);

        Task<User> GetUserAsync(string username);
        Task<User> GetUserAsync(int id);

        Task<bool> UserExistsAsync(string username);
        Task<bool> UserExistsAsync(string username, string password);

        // ✅ NEW: CRUD Helpers
        Task<IEnumerable<User>> GetAllAsync();     // Get all users
        Task<User> GetByIdAsync(int id);           // Get by ID
        void Delete(User user);                    // Delete user
    }
}
