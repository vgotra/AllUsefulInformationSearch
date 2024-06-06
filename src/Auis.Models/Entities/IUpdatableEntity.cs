namespace Auis.Models.Entities;

public interface IUpdatableEntity
{
    DateTimeOffset LastUpdated { get; set; }
}