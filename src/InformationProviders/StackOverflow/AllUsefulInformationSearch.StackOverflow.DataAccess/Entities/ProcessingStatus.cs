namespace AllUsefulInformationSearch.StackOverflow.DataAccess.Entities;

public enum ProcessingStatus
{
    Unknown = 0,
    New,
    Updated,
    InProgress,
    Processed,
    Failed
}