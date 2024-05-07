namespace AllUsefulInformationSearch.StackOverflow.Models.ApiModels;

public class WebDataFileResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Link { get; set; } = null!;
    public long Size { get; set; }
    public DateTimeOffset ExternalLastModified { get; set; }
    public ProcessingStatus ProcessingStatus { get; set; }
    public bool IsSynchronizationEnabled { get; set; }
}