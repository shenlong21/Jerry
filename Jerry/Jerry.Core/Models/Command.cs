namespace Jerry.Core.Models;

public class Command
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string CommandString { get; set; }
    public bool IsPrefixCmdRun { get; set; }
    public string? Description { get; set; }

    public List<SaltCommand>? SaltCommands { get; set; } = [];
}
