namespace AllUsefulInformationSearch.StackOverflow.Models.ApiModels;

public abstract class BasePagingRequest
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}