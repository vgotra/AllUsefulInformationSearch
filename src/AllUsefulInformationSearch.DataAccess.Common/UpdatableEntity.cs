namespace AllUsefulInformationSearch.DataAccess.Common;

public abstract class UpdatableEntity<T> : Entity<T>, IUpdatableEntity where T : struct
{
    public DateTimeOffset LastUpdated { get; set; }
}