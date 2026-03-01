using Jerry.API.Enums;

namespace Jerry.API.Models.ViewModels;

public class TaskUserVM
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public string? User { get; set; }
    public string? Hostname { get; set; }
    public string? IpAddress { get; set; }
    public SaltTaskStatus Status { get; set; }
}
