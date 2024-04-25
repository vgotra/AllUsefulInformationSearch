namespace AllUsefulInformationSearch.StackOverflow.PostsParser.Models;

public enum PostType
{
    Unknown = 0,
    Question,
    Answer,
    OrphanedTagWiki,
    TagWikiExcerpt,
    TagWiki,
    ModeratorNomination,
    WikiPlaceholder,
    PrivilegeWiki
}