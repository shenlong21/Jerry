using Jerry.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jerry.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectRepository _projectRepository;
    private readonly ILogger<ProjectController> _logger;

    public ProjectController(IProjectRepository projectRepository, ILogger<ProjectController> logger)
    {
        _projectRepository = projectRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        try
        {
            var projects = await _projectRepository.GetAllProjectsAsync();

            return Ok(projects);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all projects");
            throw;
        }
    }
}
