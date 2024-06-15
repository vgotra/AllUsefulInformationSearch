namespace Auis.StackOverflow.Services;

public interface IParsingService
{
    public Task<List<PostEntity>> ParsePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);
}