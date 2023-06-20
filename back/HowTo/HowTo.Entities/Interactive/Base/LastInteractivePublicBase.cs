using System;

namespace HowTo.Entities.Interactive.Base;

public class LastInteractivePublicBase
{
    public int InteractiveId { get; init; }
    public int CourseId { get; init; }
    public int ArticleId { get; init; }
    public InteractiveType InteractiveType { get; init; }
}