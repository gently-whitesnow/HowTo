using System.ComponentModel.DataAnnotations;

namespace HowTo.Entities.Article;

public class UpdateStatusArticleRequest
{
    [Required]
    public int? ArticleId { get; set; }
    [Required]
    public int CourseId { get; set; }
    [Required]
    public EntityStatus Status { get; set; }
}