namespace Jerry.API.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; } = string.Empty;

        // Navigation property to users
        public List<User> Users { get; set; } = [];
        public List<SaltTask> SaltTasks { get; set; } = [];
    }
}
