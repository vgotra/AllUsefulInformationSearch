namespace Auis.Wikipedia.Models.ApiModels;

public abstract class BasePagingResponse
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalRecords { get; set; }
}