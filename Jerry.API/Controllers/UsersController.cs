using Microsoft.AspNetCore.Mvc;
using Jerry.API.Repositories.Interfaces;
using Jerry.API.Models;

namespace Jerry.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userRepository.GetAllUsersAsync();
                
                
                
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Get a user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                
                if (user == null)
                {
                    return NotFound($"User with ID {id} not found");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting user with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = new User
                {
                    Name = request.Name,
                    Hostname = request.Hostname,
                    Project = request.Project,
                    IpAddress = request.IpAddress,
                    GrubPassword = request.GrubPassword,
                    Password = request.Password
                };

                var createdUser = await _userRepository.CreateUserAsync(user);
                return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }
    }

    /// <summary>
    /// Request model for creating a user
    /// </summary>
    public class CreateUserRequest
    {
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public int Project { get; set; }
        public string? IpAddress { get; set; }
        public string? GrubPassword { get; set; }
        public string? Password { get; set; }
    }
}
