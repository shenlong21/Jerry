namespace Jerry.API.Models.ViewModels;

public class CommandVM
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public bool IsPrefixCmdRun { get; set; } = true;
    public required string CommandString { get; set; }
    public string? Description { get; set; }
}
