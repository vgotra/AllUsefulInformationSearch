namespace Auis.Models.Entities;

public abstract class Entity<T> where T : struct
{
    public T Id { get; set; }
}