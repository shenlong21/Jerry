using System.Text.RegularExpressions;
using Jerry.API.Data;
using Jerry.API.Enums;
using Jerry.API.Models.Models;
using Jerry.API.Models.RequestModels;
using Jerry.API.Models.ViewModels;
using Jerry.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;


namespace Jerry.API.Repositories.Implementations
{
    public class SaltTaskRepository : ISaltTaskRepository
    {
        private readonly JerryContext _context;
        private readonly ILogger<UserRepository> _logger;

        public SaltTaskRepository(JerryContext context, ILogger<UserRepository> logger)
        {
            _context = context;
            _logger = logger;
        }


        /// <summary>
        /// Retrieves all salt  from the database.
        /// </summary>
        /// <returns>A list of SaltTaskVM objects.</returns>
        public async Task<IEnumerable<SaltTaskVM>> GetAllTasksAsync()
        {
            try
            {
                var saltCommands = await _context.SaltCommands
                    .AsNoTracking()
                    .Include(c => c.Command)
                    .Select(s => new SaltCommandVM
                    {
                        Id = s.Id,
                        SaltTaskId = s.SaltTaskId,
                        CommandId = s.CommandId,
                        Command = new CommandVM
                        {
                            Id = (s.Command != null) ? s.Command.Id : default(int),
                            Name = (s.Command != null) ? s.Command.Name : string.Empty,
                            CommandString = (s.Command != null) ? s.Command.CommandString : string.Empty,
                            Description = (s.Command != null) ? s.Command.Description : string.Empty
                        },
                        Description = s.Description
                    })
                    .ToListAsync();

                var tasks = await _context.SaltTasks
                    .AsNoTracking()
                    .Include(p => p.Project)
                    .Include(u => u.TaskUsers)
                        .ThenInclude(t => t.User)
                    .Include(c => c.SaltCommands)
                    .Select(u => new SaltTaskVM
                    {
                        Id = u.Id,
                        Title = u.Title,
                        Description = u.Description,
                        SaltSelector = u.SaltSelector,
                        SaltCommands = new List<SaltCommandVM>(),
                        Status = u.Status,
                        Project = new ProjectVM()
                        {
                            Id = (u.Project != null) ? u.Project.Id : 0,
                            Name = (u.Project != null) ? u.Project.ProjectName : string.Empty
                        },
                        TaskUsers = u.TaskUsers.Select(t => new TaskUserVM
                        {
                            TaskId = t.TaskId,
                            UserId = t.UserId,
                            User = (t.User != null) ? t.User.Name : string.Empty,
                            Hostname = (t.User != null) ? t.User.Hostname : string.Empty,
                            IpAddress = (t.User != null) ? t.User.IpAddress : string.Empty,
                            Status = t.Status,
                        }).ToList(),
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt,
                    }).ToListAsync();

                foreach (var task in tasks)
                {
                    task.SaltCommands = saltCommands.Where(s => s.SaltTaskId == task.Id).ToList();
                }
                return tasks;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        /// <summary>
        /// Retrieves all salt  from the database.
        /// </summary>
        /// <returns>A list of SaltTaskVM objects.</returns>
        public async Task<SaltTaskVM> GetTaskByIdAsync(int id)
        {
            try
            {
                var saltCommands = await _context.SaltCommands
                    .AsNoTracking()
                    .Include(c => c.Command)
                    .Where(c => c.SaltTaskId == id)
                    .Select(s => new SaltCommandVM
                    {
                        Id = s.Id,
                        SaltTaskId = s.SaltTaskId,
                        CommandId = s.CommandId,
                        Command = new CommandVM
                        {
                            Id = (s.Command != null) ? s.Command.Id : default(int),
                            Name = (s.Command != null) ? s.Command.Name : string.Empty,
                            CommandString = (s.Command != null) ? s.Command.CommandString : string.Empty,
                            Description = (s.Command != null) ? s.Command.Description : string.Empty,
                            IsPrefixCmdRun = (s.Command != null) ? s.Command.IsPrefixCmdRun : true,
                        },
                        Description = s.Description
                    })
                    .ToListAsync();

                var task = await _context.SaltTasks
                    .AsNoTracking()
                    .Where(t => t.Id == id)
                    .Include(p => p.Project)
                    .Include(u => u.TaskUsers)
                        .ThenInclude(t => t.User)
                    .Select(u => new SaltTaskVM
                    {
                        Id = u.Id,
                        Title = u.Title,
                        SaltSelector = u.SaltSelector,
                        SaltCommands = saltCommands,
                        Description = u.Description,
                        Status = u.Status,
                        Project = new ProjectVM()
                        {
                            Id = (u.Project != null) ? u.Project.Id : 0,
                            Name = (u.Project != null) ? u.Project.ProjectName : string.Empty
                        },
                        TaskUsers = u.TaskUsers.Select(t => new TaskUserVM
                        {
                            TaskId = t.TaskId,
                            UserId = t.UserId,
                            User = (t.User != null) ? t.User.Name : string.Empty,
                            Hostname = (t.User != null) ? t.User.Hostname : string.Empty,
                            IpAddress = (t.User != null) ? t.User.IpAddress : string.Empty,
                            Status = t.Status,
                        }).ToList(),
                        CreatedAt = u.CreatedAt,
                        UpdatedAt = u.UpdatedAt,
                    }).FirstOrDefaultAsync();

                if (task is not null)
                {
                    return task;
                }

                return new SaltTaskVM() { Title = "No Task", Project = new ProjectVM() { Id = 0, Name = "Default" } };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users");
                throw;
            }
        }

        public async Task<SaltTaskVM> CreateTaskAsync(CreateTaskRequestModel task)
        {
            var project = await _context.Projects
                .AsNoTracking()
                .Where(t => t.Id == task.ProjectId)
                .FirstOrDefaultAsync();

            if (project is null)
            {
                throw new ArgumentException("Project not found");
            }

            try
            {
                var newTask = new SaltTask
                {
                    Title = task.Title,
                    Description = task.Description ?? string.Empty,
                    SaltSelector = task.SaltSelector ?? string.Empty,
                    Status = SaltTaskStatus.Pending,
                    ProjectId = task.ProjectId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };

                await _context.SaltTasks.AddAsync(newTask);

                await _context.SaveChangesAsync();

                // Adding users for the task
                // All the users that are in project would be assigned in the task
                var projectUsers = await _context.Users
                    .AsNoTracking()
                    .Where(t => t.ProjectId == newTask.ProjectId)
                    .ToListAsync();

                foreach (var user in projectUsers)
                {
                    var taskUser = new TaskUser
                    {
                        TaskId = newTask.Id,
                        UserId = user.Id,
                        Status = SaltTaskStatus.Pending,
                    };
                    await _context.TaskUsers.AddAsync(taskUser);
                }

                await _context.SaveChangesAsync();

                // adding commands
                var commands = new List<Command>();

                foreach (var command in task.Commands)
                {
                    commands.Add(new Command
                    {
                        Name = HyphenateString(command).Length <= 15 ? HyphenateString(command) : HyphenateString(command)[..15],
                        CommandString = command,
                        Description = HyphenateString(command),
                    });
                }

                await _context.Commands.AddRangeAsync(commands);
                await _context.SaveChangesAsync();

                // get the commands to add them to salt commands

                var saltCommands = new List<SaltCommand>();

                foreach (var command in commands)
                {
                    saltCommands.Add(new SaltCommand
                    {
                        CommandId = command.Id,
                        SaltTaskId = newTask.Id,
                    });
                }

                await _context.SaltCommands.AddRangeAsync(saltCommands);
                await _context.SaveChangesAsync();

                var projectUsersVM = projectUsers
                    .Select(p => new TaskUserVM
                    {
                        UserId = p.Id,
                        TaskId = newTask.Id,
                        Status = SaltTaskStatus.Pending,
                        User = p.Name,
                        Hostname = p.Hostname,
                        IpAddress = p.IpAddress,
                    }).ToList();

                // Now create the result model
                var result = new SaltTaskVM
                {
                    Id = newTask.Id,
                    Title = newTask.Title,
                    Description = newTask.Description,
                    SaltSelector = newTask.SaltSelector,
                    Status = newTask.Status,
                    Project = new ProjectVM()
                    {
                        Id = (newTask.Project != null) ? newTask.Project.Id : 0,
                        Name = (newTask.Project != null) ? newTask.Project.ProjectName : string.Empty
                    },
                    TaskUsers = projectUsersVM,
                    CreatedAt = newTask.CreatedAt,
                    UpdatedAt = newTask.UpdatedAt,
                };
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating task");
                throw;
            }
        }

        public async Task<bool> TaskUpdateForOneUser(TaskUpdateForOneUserRequestModel request)
        {
            try
            {
                var task = await _context.SaltTasks.FindAsync(request.TaskId);
                if (task == null) return false;

                var user = await _context.Users.FindAsync(request.UserId);
                if (user == null) return false;

                var taskUser = await _context.TaskUsers.Where(t => t.TaskId == request.TaskId && t.UserId == request.UserId).FirstOrDefaultAsync();
                if (taskUser == null) return false;

                taskUser.Status = request.Status;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking task as complete");
                throw;
            }
        }

        public static string HyphenateString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;

            // 1. Replace all non-alphanumeric characters (including spaces) with a single hyphen
            string cleaned = Regex.Replace(input, @"[^a-zA-Z0-9]+", "-");

            // 2. Trim hyphens from the start and end, and convert to lowercase (standard for slugs)
            return cleaned.Trim('-').ToLower();
        }

    }
}
