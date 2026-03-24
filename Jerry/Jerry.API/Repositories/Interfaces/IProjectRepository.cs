using Jerry.API.Models.ViewModels;

namespace Jerry.API.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<ProjectVM>> GetAllProjectsAsync();
}
