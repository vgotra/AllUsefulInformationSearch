namespace Auis.StackOverflow.Models.ApiModels;

public abstract class BaseSortablePagingRequest : BasePagingRequest
{
    public string? SortBy { get; set; }
    public bool IsDescending { get; set; }
}