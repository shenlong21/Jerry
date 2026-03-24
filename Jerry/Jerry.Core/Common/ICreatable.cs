namespace Jerry.Core.Common;

public interface ICreatable
{
    DateTime CreatedAt { get; set; }
    int CreatedBy { get; set; }
}