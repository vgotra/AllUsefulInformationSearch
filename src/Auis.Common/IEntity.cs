namespace Auis.Common;

public interface IEntity<T> where T : struct
{
    T Id { get; set; }
}