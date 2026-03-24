using Microsoft.AspNetCore.Mvc;

namespace Jerry.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SaltCallController {
    private readonly ILogger<CommandController> _logger;

    public SaltCallController(ILogger<CommandController> logger)
    {
        _logger = logger;
    }

    // [HttpGet("GetSaltToken")]
    // public async Task<bool> GetSaltToken() {

    // }


}
