using System;
using Microsoft.EntityFrameworkCore;

namespace HowTo.Entities.Interactive.Base;

[PrimaryKey(nameof(InteractiveId), nameof(CourseId), nameof(ArticleId), nameof(UserId))]
public class LastInteractiveBase
{
    public int InteractiveId { get; set; }
    public int CourseId { get; set; }
    public int ArticleId { get; set; }
    public Guid UserId { get; set; }
}