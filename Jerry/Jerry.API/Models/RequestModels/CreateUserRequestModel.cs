using System.ComponentModel.DataAnnotations;

namespace Jerry.API.Models.RequestModels;
/// <summary>
/// Request model for creating a user
/// </summary>
public class CreateUserRequestModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Hostname { get; set; } = string.Empty;

    [Required]
    public int ProjectId { get; set; }
    public string? IpAddress { get; set; }
    public string? GrubPassword { get; set; }
    public string? Password { get; set; }
    public string? AILTag { get; set; }
}
