using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ATI.Services.Common.Behaviors;
using HowTo.Entities;
using HowTo.Entities.Interactive;
using HowTo.Entities.Interactive.Base;
using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswer;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;
using Microsoft.EntityFrameworkCore;

namespace HowTo.DataAccess.Repositories;

public class InteractiveRepository
{
    private readonly IDbContextFactory<ApplicationContext> _dbContextFactory;
    public InteractiveRepository(IDbContextFactory<ApplicationContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<OperationResult<TDto>> UpsertInteractiveAsync<TDto>(
        Expression<Func<TDto, bool>> upsertCondition,
        Func<TDto> getFunc,
        Action<TDto> updateFunc
    ) where TDto : class, new()
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var dbContext = db.Set<TDto>();
            var interactiveDto = await dbContext.SingleOrDefaultAsync(upsertCondition);
            if (interactiveDto == null)
            {
                return await InsertInteractiveAsync(getFunc);
            }

            updateFunc(interactiveDto);
            await db.SaveChangesAsync();
            return new(interactiveDto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<TDto>> InsertInteractiveAsync<TDto>(
        Func<TDto> getFunc
    ) where TDto : class, new()
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var dto = getFunc();
            await db.Set<TDto>().AddAsync(dto);
            await db.SaveChangesAsync();
            return new(dto);
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<InteractivePublic>> GetInteractiveAsync(int courseId, int articleId, bool isAuthor)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var checkListDto = await db.CheckListContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();
            var choiceOfAnswerDto = await db.ChoiceOfAnswerContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();
            var programWritingDto = await db.ProgramWritingContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();
            var writingOfAnswerDto = await db.WritingOfAnswerContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();

            return new(new InteractivePublic(
                checkListDto.Select(dto => new CheckListPublic(dto)).ToArray(),
                choiceOfAnswerDto.Select(dto => new ChoiceOfAnswerPublic(dto)).ToArray(),
                programWritingDto.Select(dto => new ProgramWritingPublic(dto)).ToArray(),
                writingOfAnswerDto.Select(dto => new WritingOfAnswerPublic(dto, isAuthor)).ToArray()));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<LastInteractivePublic>> GetLastInteractiveAsync(int courseId, int articleId, User user)
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var checkListDto = await GetLastInteractiveDtoAsync<LastCheckListDto>();
            var choiceOfAnswerDto =await GetLastInteractiveDtoAsync<LastChoiceOfAnswerDto>();
            var programWritingDto = await GetLastInteractiveDtoAsync<LastProgramWritingDto>();
            var writingOfAnswerDto = await GetLastInteractiveDtoAsync<LastWritingOfAnswerDto>();

            return new(new LastInteractivePublic(
                checkListDto.Select(d=>new LastCheckListPublic(d)).ToArray(),
                choiceOfAnswerDto.Select(d=>new LastChoiceOfAnswerPublic(d)).ToArray(),
                programWritingDto.Select(d=>new LastProgramWritingPublic(d)).ToArray(),
                writingOfAnswerDto.Select(d=>new LastWritingOfAnswerPublic(d)).ToArray()));

            async Task<TInteractiveDto[]> GetLastInteractiveDtoAsync<TInteractiveDto>() where TInteractiveDto : LastInteractiveBase 
                => await db.Set<TInteractiveDto>().AsQueryable()
                    .Where(a => a.CourseId == courseId && a.ArticleId == articleId && a.UserId == user.Id).ToArrayAsync();
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<TDto>> GetInteractiveByIdAsync<TDto>(int interactiveId)
        where TDto : class, IHaveId
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var dbContext = db.Set<TDto>();
            var interactiveDto = await dbContext.SingleOrDefaultAsync(c => c.Id == interactiveId);

            return interactiveDto != null
                ? new(interactiveDto)
                : new(Errors.InteractiveNotFound(interactiveId));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public async Task<OperationResult<InteractiveByIdPublic>> DeleteInteractiveByIdAsync<TDto>(int interactiveId)
        where TDto : class, IHaveId
    {
        try
        {
            using var db = _dbContextFactory.CreateDbContext();
            var dbContext = db.Set<TDto>();
            dynamic interactiveDto = await dbContext.SingleOrDefaultAsync(c => c.Id == interactiveId);

            if (interactiveDto == null)
                return new(Errors.InteractiveNotFound(interactiveId));

            dbContext.Remove(interactiveDto);
            await db.SaveChangesAsync();
            return new(new InteractiveByIdPublic(interactiveDto));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
}