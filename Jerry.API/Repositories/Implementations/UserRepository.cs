using Jerry.API.Data;
using Jerry.API.Models;
using Jerry.API.Repositories.Interfaces;
using Jerry.API.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Jerry.API.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly JerryContext _context;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(JerryContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserVM>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .AsNoTracking()
                    .Include(p => p.ProjectNavigation)
                    .Select(u => new UserVM
                {
                    id = u.Id,
                    Hostname =  u.Hostname,
                    GrubPassword =  u.GrubPassword,
                    IpAddress = u.IpAddress,
                    LastConnected = u.LastConnected,
                    Name = u.Name,
                    Password = u.Password,
                    Project = u.ProjectNavigation
                }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                throw;
            }
        }

        public async Task<User> CreateUserAsync(User user)
        {
            try
            {
                user.Id = Guid.NewGuid();
                user.LastConnected = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            try
            {
                user.LastConnected = DateTime.UtcNow;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {user.Id}");
                throw;
            }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return false;

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}");
                throw;
            }
        }

        public async Task<User?> GetUserByHostnameAsync(string hostname)
        {
            try
            {
                return await _context.Users.FirstOrDefaultAsync(u => u.Hostname == hostname);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with hostname {hostname}");
                throw;
            }
        }
    }
}
