using Jerry.API.Models.ViewModels;
using Jerry.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Jerry.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommandController : ControllerBase
{
    private readonly ICommandRepository _commandRepository;
    private readonly ILogger<CommandController> _logger;

    public CommandController(ICommandRepository commandRepository, ILogger<CommandController> logger)
    {
        _commandRepository = commandRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCommandsAsync()
    {
        try
        {
            var commands = await _commandRepository.GetAllCommandsAsync();

            return Ok(commands);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddCommandAsync(CommandVM model)
    {
        try
        {
            var command = await _commandRepository.AddCommandAsync(model);

            // return CreatedAtAction(nameof(GetCommandByIdAsync), new { id = command.Id }, command);
            return Ok(command);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}