namespace Jerry.API.Models.ViewModels
{
    public class UserVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Hostname { get; set; } = string.Empty;
        public string Project { get; set; } = string.Empty;
        public string? IpAddress { get; set; }
        public string? GrubPassword { get; set; }
        public string? Password { get; set; }
        public string? AILTag { get; set; }
        public DateTime? LastConnected { get; set; }
    }
}
