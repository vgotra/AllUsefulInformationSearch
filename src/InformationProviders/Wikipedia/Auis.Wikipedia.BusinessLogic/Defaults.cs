namespace Auis.Wikipedia.BusinessLogic;

public static class Defaults
{
    /// <summary>
    /// Block all russian information because useless
    /// </summary>
    public static readonly List<string> AdditionalFileNamesToSkip =
    [
        "ru.meta.stackoverflow.com.7z",
        "ru.stackoverflow.com.7z",
        "rus.meta.stackexchange.com.7z",
        "rus.stackexchange.com.7z",
        "russian.meta.stackexchange.com.7z",
        "russian.stackexchange.com.7z"
    ];

    /// <summary>
    /// List of files with aggregated information that should be skipped
    /// Easier to track changes by files split by categories
    /// </summary>
    public static readonly List<string> FileNamesToSkip =
    [
        "stackoverflow.com-Badges.7z",
        "stackoverflow.com-PostLinks.7z",
        "stackoverflow.com-Tags.7z",
        "stackoverflow.com-Users.7z",
        "stackoverflow.com-Comments.7z",
        "stackoverflow.com-Votes.7z",
        "stackoverflow.com-PostHistory.7z",
        "stackoverflow.com-Posts.7z"
    ];

    public static List<string> AllFileNamesToSkip => FileNamesToSkip.Concat(AdditionalFileNamesToSkip).ToList();

    public static List<PostType> UsefulPostTypes => [PostType.Question, PostType.Answer, PostType.TagWiki];
}