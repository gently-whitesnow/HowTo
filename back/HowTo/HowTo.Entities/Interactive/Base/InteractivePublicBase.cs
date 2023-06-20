namespace HowTo.Entities.Interactive.Base;

public class InteractivePublicBase
{
    public int Id { get; init; }
    public int CourseId { get; init; }
    public int ArticleId { get; init; }
    public string Description { get; init; }
    public InteractiveType InteractiveType { get; init; }
}