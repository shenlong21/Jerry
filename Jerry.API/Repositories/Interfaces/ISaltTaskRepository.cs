using Jerry.API.Models.Models;
using Jerry.API.Models.RequestModels;
using Jerry.API.Models.ViewModels;

namespace Jerry.API.Repositories.Interfaces
{
    public interface ISaltTaskRepository
    {
        Task<IEnumerable<SaltTaskVM>> GetAllTasksAsync();
        Task<SaltTaskVM> GetTaskByIdAsync(int id);
        Task<SaltTaskVM> CreateTaskAsync(CreateTaskRequestModel task);
        Task<bool> TaskUpdateForOneUser(TaskUpdateForOneUserRequestModel request);
        // Task<Task> UpdateTaskAsync(int id, UpdateTaskRequestModel task);
        // Task<bool> DeleteTaskAsync(int id);
    }
}
