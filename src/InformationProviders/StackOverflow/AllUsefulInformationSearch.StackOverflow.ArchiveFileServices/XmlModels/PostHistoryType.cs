namespace AllUsefulInformationSearch.StackOverflow.PostsParser.XmlModels;

public enum PostHistoryType
{
    Unknown = 0,
    InitialTitle, // initial title (questions only)
    InitialBody, // initial post raw body text
    InitialTags, // initial list of tags (questions only)
    EditTitle, // modified title (questions only)
    EditBody, // modified post body (raw markdown)
    EditTags, // modified list of tags (questions only)
    RollbackTitle, // reverted title (questions only)
    RollbackBody, // reverted body (raw markdown)
    RollbackTags, // reverted list of tags (questions only)
    PostClosed, // post voted to be closed
    PostReopened, // post voted to be reopened
    PostDeleted, // post voted to be removed
    PostUndeleted, // post voted to be restored
    PostLocked, // post locked by moderator
    PostUnlocked, // post unlocked by moderator
    CommunityOwned, // post now community owned
    PostMigrated, // post migrated, now replaced by 35/36 (away/here)
    QuestionMerged, // question merged with deleted question
    QuestionProtected, // question was protected by a moderator.
    QuestionUnprotected, // question was unprotected by a moderator.
    PostDisassociated, // OwnerUserId removed from post by admin
    QuestionUnmerged, // answers/votes restored to previously merged question
    UnknownDevRelatedEvent, // Unknown dev related event, not present anymore
    SuggestedEditApplied,
    PostTweeted,
    VoteNullificationByDev, // Vote nullification by dev, not present anymore
    PostUnmigratedOrHiddenByModeratorMigration, // Post unmigrated/hidden moderator migration, not present anymore
    UnknownSuggestionEvent, // Unknown suggestion event, not present anymore
    UnknownModeratorEvent, // Unknown moderator event (possibly de-wikification?), not present anymore
    UnknownEvent, // Unknown Event (too rare to guess), not present anymore
    CommentDiscussionMovedToChat, // Comment discussion moved to chat
    PostNoticeAdded = 33, // comment contains foreign key to PostNotices
    PostNoticeRemoved, // comment contains foreign key to PostNotices
    PostMigratedAway, // replaces id 17
    PostMigratedHere, // replaces id 17
    PostMergeSource,
    PostMergeDestination,
    BumpedByCommunityUser = 50, // Bumped by Community User
    QuestionBecameHotNetworkQuestion = 52, // Question became hot network question (main) / Hot Meta question (meta)
    QuestionRemovedFromHotNetwork = 53, // Question removed from hot network/meta questions by a moderator
    CreatedFromAskWizard = 66 // Created from Ask Wizard
}