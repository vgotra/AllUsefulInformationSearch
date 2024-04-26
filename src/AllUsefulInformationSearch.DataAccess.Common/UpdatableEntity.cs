namespace AllUsefulInformationSearch.DataAccess.Common;

public abstract class UpdatableEntity<T> : Entity<T> where T : struct
{
    public DateTimeOffset LastUpdated { get; set; }
}