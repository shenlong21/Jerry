using Jerry.API.Models.Models;
using Jerry.API.Models.RequestModels;
using Jerry.API.Models.ViewModels;

namespace Jerry.API.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserVM>> GetAllUsersAsync();
        Task<UserVM> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(CreateUserRequestModel user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<User?> GetUserByHostnameAsync(string hostname);
    }
}
