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
    private readonly ApplicationContext _db;

    public InteractiveRepository(ApplicationContext applicationContext)
    {
        _db = applicationContext;
    }

    public async Task<OperationResult<TDto>> UpsertInteractiveAsync<TDto>(
        Expression<Func<TDto, bool>> upsertCondition,
        Func<TDto> getFunc,
        Action<TDto> updateFunc
    ) where TDto : class, new()
    {
        try
        {
            var dbContext = _db.Set<TDto>();
            var interactiveDto = await dbContext.SingleOrDefaultAsync(upsertCondition);
            if (interactiveDto == null)
            {
                return await InsertInteractiveAsync(getFunc);
            }

            updateFunc(interactiveDto);
            await _db.SaveChangesAsync();
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
            var dto = getFunc();
            await _db.Set<TDto>().AddAsync(dto);
            await _db.SaveChangesAsync();
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
            var checkListDto = await _db.CheckListContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();
            var choiceOfAnswerDto = await _db.ChoiceOfAnswerContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();
            var programWritingDto = await _db.ProgramWritingContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();
            var writingOfAnswerDto = await _db.WritingOfAnswerContext.AsQueryable()
                .Where(a => a.CourseId == courseId && a.ArticleId == articleId).ToArrayAsync();

            return new(new InteractivePublic(
                checkListDto.Select(dto => new CheckListPublic(dto)).ToArray(),
                choiceOfAnswerDto.Select(dto => new ChoiceOfAnswerPublic(dto, isAuthor)).ToArray(),
                programWritingDto.Select(dto => new ProgramWritingPublic(dto, isAuthor)).ToArray(),
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
                => await _db.Set<TInteractiveDto>().AsQueryable()
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
            var dbContext = _db.Set<TDto>();
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
            var dbContext = _db.Set<TDto>();
            dynamic interactiveDto = await dbContext.SingleOrDefaultAsync(c => c.Id == interactiveId);

            if (interactiveDto == null)
                return new(Errors.InteractiveNotFound(interactiveId));

            dbContext.Remove(interactiveDto);
            await _db.SaveChangesAsync();
            return new(new InteractiveByIdPublic(interactiveDto));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
}