namespace Jerry.API.ViewModels
{
    public class UserVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public ProjectVM? Project { get; set; }
        public string? IpAddress { get; set; }
        public string? GrubPassword { get; set; }
        public string? Password { get; set; }
        public DateTime LastConnected { get; set; }
    }
}

