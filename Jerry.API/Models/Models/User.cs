namespace Jerry.API.Models.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public string? IpAddress { get; set; }
        public string? GrubPassword { get; set; }
        public string? Password { get; set; }
        public string? AILTag { get; set; }
        public DateTime? LastConnected { get; set; }

        // Foreign key navigation property
        public Project? Project { get; set; }
        public List<TaskUser> TaskUsers { get; set; } = [];
    }
}
