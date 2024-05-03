namespace AllUsefulInformationSearch.Models.Entities;

public interface IUpdatableEntity
{
    DateTimeOffset LastUpdated { get; set; }
}