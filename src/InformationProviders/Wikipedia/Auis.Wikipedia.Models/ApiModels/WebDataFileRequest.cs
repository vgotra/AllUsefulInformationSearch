namespace Auis.Wikipedia.Models.ApiModels;

public sealed class WebDataFileRequest : BaseSortablePagingRequest
{
    public string? Name { get; set; }
}