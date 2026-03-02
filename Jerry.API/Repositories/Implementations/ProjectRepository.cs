using Jerry.API.Data;
using Jerry.API.Models.ViewModels;
using Jerry.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Jerry.API.Repositories.Implementations;

public class ProjectRepository : IProjectRepository
{
    private readonly JerryContext _context;
    private readonly ILogger<ProjectRepository> _logger;

    public ProjectRepository(JerryContext context, ILogger<ProjectRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<ProjectVM>> GetAllProjectsAsync()
    {
        try
        {
            var projects = await _context.Projects.AsNoTracking().Select(p => new ProjectVM
            {
                Id = p.Id,
                Name = p.ProjectName
            }).ToListAsync();

            return projects;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }
}
