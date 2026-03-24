using Jerry.API.Models.Models;

namespace Jerry.API.Models.ViewModels;

public class SaltCommandVM
{
    public int Id { get; set; }
    public int SaltTaskId { get; set; }
    public int CommandId { get; set; }
    public required CommandVM Command { get; set; }
    public string? Description { get; set; }
}
    