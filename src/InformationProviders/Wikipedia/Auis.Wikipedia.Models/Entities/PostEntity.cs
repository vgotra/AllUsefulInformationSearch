namespace Auis.Wikipedia.Models.Entities;

public class PostEntity : Entity<int>
{
    public int WebDataFileId { get; set; }
    public string Title { get; set; } = null!;
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public DateTimeOffset QuestionExternalLastActivityDate { get; set; }
    public DateTimeOffset AnswerExternalLastActivityDate { get; set; }

    public WebDataFileEntity WebDataFile { get; set; } = null!;

    public int AcceptedAnswerId { get; set; }
}