namespace Jerry.API.Models.Models;

public class SaltCommand
{
    public int Id { get; set; }
    public int SaltTaskId { get; set; }
    public int CommandId { get; set; }
    public string? Description { get; set; }

    public Command? Command { get; set; }
    public SaltTask? SaltTask { get; set; }
}
