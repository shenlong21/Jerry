using Jerry.API.Enums;

namespace Jerry.API.Models.Models;

public class SaltTask
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string SaltSelector { get; set; } = string.Empty;
    public SaltTaskStatus Status { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Project? Project { get; set; }
    public List<TaskUser> TaskUsers { get; set; } = [];
    public List<SaltCommand>? SaltCommands { get; set; } = [];

}
