﻿namespace Auis.StackOverflow.Models.Entities;

public class PostEntity : Entity<int>
{
    public string Title { get; set; } = null!;
    public string Text { get; set; } = null!;
    public DateTimeOffset ExternalLastActivityDate { get; set; }
    public int WebDataFileId { get; set; }

    public AcceptedAnswerEntity AcceptedAnswer { get; set; } = null!;
    public WebDataFileEntity WebDataFile { get; set; } = null!;
}