namespace Jerry.Core.Common;

public interface ITrackable: ICreatable
{
    DateTime UpdatedAt { get; set; }
    int UpdatedBy { get; set; }
}