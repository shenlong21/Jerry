using Jerry.API.Enums;

namespace Jerry.API.Models.RequestModels;

public class TaskUpdateForOneUserRequestModel
{
    public int TaskId { get; set; }
    public int UserId { get; set; }
    public SaltTaskStatus Status { get; set; }
}
