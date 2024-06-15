namespace Auis.StackOverflow.BusinessLogic;

public interface IParsingService
{
    public Task<List<PostEntity>> ParsePostsAsync(WebFileInformation webFileInformation, CancellationToken cancellationToken = default);
}