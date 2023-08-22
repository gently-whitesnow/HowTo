using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ATI.Services.Common.Behaviors;
using HowTo.Entities;
using HowTo.Entities.Contributor;
using HowTo.Entities.Course;
using Microsoft.EntityFrameworkCore;

namespace HowTo.DataAccess.Repositories;

public class CourseRepository
{
    private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;
    public CourseRepository(IDbContextFactory<ApplicationContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<OperationResult<CourseDto>> InsertCourseAsync(UpsertCourseRequest request, User user)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext.SingleOrDefaultAsync(c => c.Title == request.Title);
            if (courseDto != null)
                return new (Errors.CourseTitleAlreadyExist(courseDto.Title));

            var dto = new CourseDto
            {
                Author = new ContributorEntity
                {
                    UserId = user.Id,
                    Name = user.Name
                },
                Title = request.Title,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await db.CourseContext.AddAsync(dto);
            await db.SaveChangesAsync();
            return new (dto);
        }
        catch (Exception ex)
        {
            return new (ex);
        }
    }

    public async Task<OperationResult<CourseDto>> UpdateCourseAsync(UpsertCourseRequest request)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext.Include(c=>c.Author)
                .FirstOrDefaultAsync(c => c.Id == request.CourseId);
            if (courseDto == null)
                return new(Errors.CourseNotFound(request.CourseId!.Value));

            courseDto.UpdatedAt = DateTime.UtcNow;
            courseDto.Title = request.Title;
            courseDto.Description = request.Description;

            await db.SaveChangesAsync();
            return new(courseDto);
        }
        catch (Exception ex)
        {
            return new OperationResult<CourseDto>(ex);
        }
    }
    
    public async Task<OperationResult<CourseDto>> UpdateStatusCourseAsync(UpdateStatusCourseRequest request)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext.SingleOrDefaultAsync(c => c.Id == request.CourseId);
            if (courseDto == null)
                return new(Errors.CourseNotFound(request.CourseId));

            courseDto.UpdatedAt = DateTimeOffset.Now;
            courseDto.Status = request.Status;

            await db.SaveChangesAsync();
            return new(courseDto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    
    public async Task<OperationResult<CourseDto>> GetCourseByIdAsync(int courseId)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext
                .Include(d=>d.Author)
                .Include(c => c.Articles)
                .ThenInclude(a=>a.Author)
                .SingleOrDefaultAsync(c => c.Id == courseId);
            if (courseDto == null)
                return new(Errors.CourseNotFound(courseId));

            return new(courseDto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<CourseDto>> DeleteCourseByIdAsync(int courseId)
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            var courseDto = await db.CourseContext
                .Include(d=>d.Author)
                .Include(d=>d.Articles)
                .ThenInclude(a=>a.Author)
                .SingleOrDefaultAsync(c => c.Id == courseId);
            if (courseDto == null)
                return new(Errors.CourseNotFound(courseId));

            db.CourseContext.Remove(courseDto);
            foreach (var articleDto in courseDto.Articles)
            {
                db.ArticleContext.Remove(articleDto);
            }
            await db.SaveChangesAsync();
            return new(courseDto);
        }
        catch (Exception ex)
        {
            return new OperationResult<CourseDto>(ex);
        }
    }

    public async Task<OperationResult<List<CourseDto>>> GetAllCoursesAsync()
    {
        try
        {
            await using var db = await _dbContextFactory.CreateDbContextAsync();
            return new(await db.CourseContext
                .Include(d=>d.Author)
                .Include(d=>d.Articles)
                .ThenInclude(a=>a.Author)
                .ToListAsync());
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
}