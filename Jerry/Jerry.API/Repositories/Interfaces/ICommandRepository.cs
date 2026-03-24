using Jerry.API.Models.ViewModels;

namespace Jerry.API.Repositories.Interfaces;

public interface ICommandRepository
{
    Task<IEnumerable<CommandVM>> GetAllCommandsAsync();
    // Task<Command> GetCommandByIdAsync(int id);
    Task<bool> AddCommandAsync(CommandVM command);
    // Task UpdateCommandAsync(Command command);
    // Task DeleteCommandAsync(int id);
}
