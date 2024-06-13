namespace Auis.StackOverflow.Models.Entities;

public class PostEntity : Entity<int>
{
    public int WebDataFileId { get; set; }
    public string Title { get; set; } = null!;
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
    //TODO What if we need only last activity date for any of them? Check this out
    public DateTimeOffset QuestionExternalLastActivityDate { get; set; }
    public DateTimeOffset AnswerExternalLastActivityDate { get; set; }

    public WebDataFileEntity WebDataFile { get; set; } = null!;
}