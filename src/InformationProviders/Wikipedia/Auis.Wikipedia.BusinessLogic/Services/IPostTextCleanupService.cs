namespace Auis.Wikipedia.BusinessLogic.Services;

public interface IPostTextCleanupService
{
    public string CleanupHtmlText(string text);
}