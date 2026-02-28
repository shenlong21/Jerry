using Jerry.API.Enums;

namespace Jerry.API.Models;

public class TaskUser
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public SaltTaskStatus Status { get; set; }

    public SaltTask? SaltTask { get; set; }
    public User? User { get; set; }
}