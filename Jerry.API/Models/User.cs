namespace Jerry.API.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public int Project { get; set; }
        public string? IpAddress { get; set; }
        public string? GrubPassword { get; set; }
        public string? Password { get; set; }
        public DateTime LastConnected { get; set; }

        // Foreign key navigation property
        public Project? ProjectNavigation { get; set; }
    }
}
