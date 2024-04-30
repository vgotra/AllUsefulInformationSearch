namespace AllUsefulInformationSearch.DataAccess.Common;

public interface IUpdatableEntity
{
    DateTimeOffset LastUpdated { get; set; }
}