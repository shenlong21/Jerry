namespace Jerry.Core.Common;

public interface IDeletable
{
    bool IsDeleted { get; set; }
    int DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }
}