namespace Jerry.API.Models.RequestModels
{
    public class CreateTaskRequestModel
    {
        public required string Title { get; set; }
        public string? Description { get; set; }
        // public string? SaltSelector { get; set; }
        public int ProjectId { get; set; }
        public int[] Commands { get; set; } = [];
    }
}
