using Jerry.API.Data;
using Jerry.API.Models.Models;
using Jerry.API.Repositories.Interfaces;
using Jerry.API.Models.ViewModels;
using Jerry.API.Models.RequestModels;
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

        /// <summary>
        /// Retrieves all users from the database.
        /// </summary>
        /// <returns>A list of UserVM objects.</returns>
        public async Task<IEnumerable<UserVM>> GetAllUsersAsync()
        {
            try
            {
                return await _context.Users
                    .AsNoTracking()
                    .Include(p => p.Project)
                    .Select(u => new UserVM
                    {
                        Id = u.Id,
                        Hostname = u.Hostname,
                        GrubPassword = u.GrubPassword,
                        IpAddress = u.IpAddress,
                        LastConnected = u.LastConnected,
                        Name = u.Name,
                        AILTag = u.AILTag,
                        Password = u.Password,
                        Project = (u.Project != null) ? u.Project.ProjectName : string.Empty
                    }).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<UserVM> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await _context.Users
                    .Include(p => p.Project)
                    .AsNoTracking()
                    .Where(u => u.Id == id).Select(u => new UserVM
                    {
                        Id = u.Id,
                        Name = u.Name,
                        Hostname = u.Hostname,
                        Project = (u.Project != null) ? u.Project.ProjectName : string.Empty,
                        IpAddress = u.IpAddress,
                        GrubPassword = u.GrubPassword,
                        Password = u.Password,
                        AILTag = u.AILTag,
                        LastConnected = u.LastConnected
                    }).FirstOrDefaultAsync();

                if (user is not null)
                {
                    return user;
                }

                throw new Exception($"User with ID {id} not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                throw;
            }
        }

        public async Task<User> CreateUserAsync(CreateUserRequestModel userRequest)
        {
            try
            {
                var project = await _context.Projects
                    .AsNoTracking()
                    .Where(p => p.Id == userRequest.ProjectId)
                    .FirstOrDefaultAsync();

                if (project is null)
                {
                    throw new Exception($"Project with ID {userRequest.ProjectId} not found");
                }

                var newUser = new User
                {
                    Name = userRequest.Name,
                    Hostname = userRequest.Hostname,
                    ProjectId = userRequest.ProjectId,
                    IpAddress = userRequest.IpAddress,
                    GrubPassword = userRequest.GrubPassword,
                    Password = userRequest.Password,
                    AILTag = userRequest.AILTag,
                    LastConnected = DateTime.UtcNow
                };

                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                throw;
            }
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            return null;
            // try
            // {
            //     user.LastConnected = DateTime.UtcNow;
            //     _context.Users.Update(user);
            //     await _context.SaveChangesAsync();
            //     return user;
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, $"Error updating user with ID {user.Id}");
            //     throw;
            // }
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return false;
            // try
            // {
            //     var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            //     if (user == null)
            //         return false;

            //     _context.Users.Remove(user);
            //     await _context.SaveChangesAsync();
            //     return true;
            // }
            // catch (Exception ex)
            // {
            //     _logger.LogError(ex, $"Error deleting user with ID {id}");
            //     throw;
            // }
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
