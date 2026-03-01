using Jerry.API.Enums;

namespace Jerry.API.Models.ViewModels
{
    public class SaltTaskVM
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public string? Description { get; set; }
        public string? SaltSelector { get; set; }
        public List<SaltCommandVM> SaltCommands { get; set; } = [];
        public SaltTaskStatus Status { get; set; }
        public required ProjectVM Project { get; set; }
        public List<TaskUserVM> TaskUsers { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
