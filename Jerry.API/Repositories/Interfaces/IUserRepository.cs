using Jerry.API.Models;
using Jerry.API.ViewModels;

namespace Jerry.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserVM>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(Guid id);
        Task<User> CreateUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<User?> GetUserByHostnameAsync(string hostname);
    }
}
    