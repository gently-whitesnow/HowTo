using System;
using System.Linq;
using HowTo.Entities.Contributor;
using HowTo.Entities.UserInfo;

namespace HowTo.Entities.Article;

public class ArticlePublic
{
    public ArticlePublic(ArticleDto articleDto, User user, UserUniqueInfoDto? userUniqueInfo)
    {
        Id = articleDto.Id;
        CourseId = articleDto.CourseId;
        Title = articleDto.Title;
        CreatedAt = articleDto.CreatedAt;
        UpdatedAt = articleDto.UpdatedAt;
        Status = articleDto.Status;
        Author = articleDto.Author;
        IsAuthor = articleDto.Author.UserId == user.Id;
        IsViewed = userUniqueInfo?.ApprovedViewArticleIds.Any(a=>a.ArticleId == Id && a.CourseId == CourseId)
                   ?? false;
    }
    public int Id { get;}

    public int CourseId { get;  }
    
    public string Title { get; }

    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }
    public ContributorEntity Author { get; }
    
    public EntityStatus Status { get; }
    public bool IsAuthor { get; }
    public bool IsViewed { get; }
}