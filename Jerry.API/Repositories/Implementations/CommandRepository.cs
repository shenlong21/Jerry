using Jerry.API.Data;
using Jerry.API.Models.Models;
using Jerry.API.Models.ViewModels;
using Jerry.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jerry.API.Repositories.Implementations;

public class CommandRepository : ICommandRepository
{
    private readonly JerryContext _context;
    private readonly ILogger<CommandRepository> logger;

    public CommandRepository(JerryContext jerryContext, ILogger<CommandRepository> logger)
    {
        this._context = jerryContext;
        this.logger = logger;
    }

    public async Task<IEnumerable<CommandVM>> GetAllCommandsAsync()
    {
        try
        {
            var commands = await _context.Commands
                .AsNoTracking()
                .Select(command => new CommandVM
                {
                    Id = command.Id,
                    Name = command.Name,
                    Description = command.Description,
                    CommandString = command.CommandString,
                    IsPrefixCmdRun = command.IsPrefixCmdRun
                }).ToListAsync();

            return commands;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching all commands");
            throw;
        }
    }
    
    public async Task<bool> AddCommandAsync(CommandVM command)
    {
        try
        {
            // check if another same command in db
            var commandCheck = await _context.Commands
                .AsNoTracking()
                .Where(c => c.CommandString.Equals(command.CommandString))
                .FirstOrDefaultAsync();
                
            if (commandCheck is not null) {
                return false;
            }
            
            var newCommand = new Command
            {
                Name = command.Name,
                CommandString = command.CommandString,
                IsPrefixCmdRun = command.IsPrefixCmdRun,
                Description = command.Description
            };

            await _context.Commands.AddAsync(newCommand);
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding command");
            throw;
        }
    }
}