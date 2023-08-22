using System;
using System.Linq;
using System.Threading.Tasks;
using ATI.Services.Common.Behaviors;
using HowTo.Entities;
using HowTo.Entities.Article;
using HowTo.Entities.Contributor;
using Microsoft.EntityFrameworkCore;

namespace HowTo.DataAccess.Repositories;

public class ArticleRepository
{
    private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;
    public ArticleRepository(IDbContextFactory<ApplicationContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<OperationResult<ArticleDto>> UpsertArticleAsync(UpsertArticleRequest request, User user)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext
                .Include(d => d.Articles)
                .ThenInclude(a => a.Author)
                .SingleOrDefaultAsync(c => c.Id == request.CourseId);
            if (courseDto == null)
                return new(Errors.CourseNotFound(request.CourseId));

            if (request.ArticleId == null)
            {
                var dto = new ArticleDto
                {
                    CourseId = courseDto.Id,
                    Title = request.Title,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Author = new ContributorEntity
                    {
                        UserId = user.Id,
                        Name = user.Name
                    },
                    Status = EntityStatus.Moderation
                };
                courseDto.Articles.Add(dto);
                await db.SaveChangesAsync();
                return new(dto);
            }
            
            var article = courseDto.Articles.FirstOrDefault(a => a.Id == request.ArticleId);
            if (article == null)
                return new(Errors.ArticleNotFound(request.CourseId, request.ArticleId.Value));
            
            courseDto.UpdatedAt = DateTimeOffset.Now;
            
            article.UpdatedAt = DateTime.UtcNow;
            article.Title = request.Title;

            await db.SaveChangesAsync();
            return new(article);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    
    public async Task<OperationResult<ArticleDto>> UpdateStatusArticleAsync(UpdateStatusArticleRequest request)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext
                .Include(d => d.Articles)
                .SingleOrDefaultAsync(c => c.Id == request.CourseId);
            if (courseDto == null)
                return new(Errors.CourseNotFound(request.CourseId));

            var article = courseDto.Articles.FirstOrDefault(a => a.Id == request.ArticleId);
            if (article == null)
                return new(Errors.ArticleNotFound(request.CourseId, request.ArticleId.Value));
            
            courseDto.UpdatedAt = DateTimeOffset.Now;
            
            article.UpdatedAt = DateTime.UtcNow;
            article.Status = request.Status;

            await db.SaveChangesAsync();
            return new(article);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<ArticleDto>> GetArticleByIdAsync(int courseId, int articleId)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var articleDto = await db.ArticleContext
                .Include(d => d.Author)
                .SingleOrDefaultAsync(a => a.Id == articleId && a.CourseId == courseId);
            if (articleDto == null)
                return new(Errors.ArticleNotFound(courseId, articleId));
            
            return new(articleDto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<ArticleDto>> DeleteArticleByIdAsync(int courseId, int articleId)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var articleDto =
                await db.ArticleContext
                    .Include(d=>d.Author)
                    .SingleOrDefaultAsync(c => c.CourseId == courseId && c.Id == articleId);
            if (articleDto == null)
            {
                return new(Errors.ArticleNotFound(courseId, articleId));
            }

            db.ArticleContext.Remove(articleDto);
            await db.SaveChangesAsync();
            return new(articleDto);
        }
        catch (Exception ex)
        {
            return new OperationResult<ArticleDto>(ex);
        }
    }
}