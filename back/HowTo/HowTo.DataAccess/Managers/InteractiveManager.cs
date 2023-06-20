using System;
using System.Linq;
using System.Threading.Tasks;
using ATI.Services.Common.Behaviors;
using ATI.Services.Common.Extensions;
using ATI.Services.Common.Extensions.OperationResult;
using HowTo.DataAccess.Repositories;
using HowTo.Entities;
using HowTo.Entities.Interactive;
using HowTo.Entities.Interactive.Base;
using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswer;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;
using Newtonsoft.Json;

namespace HowTo.DataAccess.Managers;

public class InteractiveManager
{
    private readonly InteractiveRepository _interactiveRepository;
    
    private readonly ArticleRepository _articleRepository;

    public InteractiveManager(
        InteractiveRepository interactiveRepository,
        ArticleRepository articleRepository)
    {
        _interactiveRepository = interactiveRepository;
        _articleRepository = articleRepository;
    }

    public async Task<OperationResult<InteractiveResponse>> GetInteractiveAsync(
        int courseId,
        int articleId,
        User user)
    {
        var articleOperation = await _articleRepository.GetArticleByIdAsync(courseId, articleId);
        if (!articleOperation.Success)
            return new(articleOperation);
        var isAuthor = articleOperation.Value.Author.UserId == user.Id;
        var interactiveOperation = await _interactiveRepository.GetInteractiveAsync(
            courseId, articleId, isAuthor);
        if (!interactiveOperation.Success)
            return new(interactiveOperation);
        
        var lastInteractiveOperation = await _interactiveRepository.GetLastInteractiveAsync(courseId, articleId, user);
        if (!lastInteractiveOperation.Success)
            return new(lastInteractiveOperation);
        
        return new(new InteractiveResponse(interactiveOperation.Value, lastInteractiveOperation.Value));
    }
    
    public async Task<OperationResult<InteractiveByIdPublic>> UpsertInteractiveAsync(UpsertInteractiveRequest request)
    {
        var articleOperation = await _articleRepository.GetArticleByIdAsync(request.CourseId, request.ArticleId);
        if (!articleOperation.Success)
            return new(articleOperation);

        var interactive = GetInteractive(request);
        dynamic interactiveOperation = interactive switch
        {
            InteractiveType.CheckList => await _interactiveRepository.UpsertInteractiveAsync(
                dto => dto.Id == request.InteractiveId,
                () => new CheckListDto
                {
                    ArticleId = request.ArticleId,
                    CourseId = request.CourseId,
                    Description = request.Description,
                    ClausesJsonStringArray = JsonConvert.SerializeObject(request.UpsertCheckList.Clauses),
                },
                dto =>
                {
                    dto.Description = request.Description;
                    dto.ClausesJsonStringArray = JsonConvert.SerializeObject(request.UpsertCheckList.Clauses);
                }),
            
            
            InteractiveType.ChoiceOfAnswer => await _interactiveRepository.UpsertInteractiveAsync(
                dto => dto.Id == request.InteractiveId,
                () => new ChoiceOfAnswerDto
                    {
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,

                        Description = request.Description,
                        QuestionsJsonStringArray =
                            JsonConvert.SerializeObject(request.UpsertChoiceOfAnswer.Questions),
                        AnswersJsonBoolArray = JsonConvert.SerializeObject(request.UpsertChoiceOfAnswer.Answers),
                    },
                dto =>
                {
                    dto.Description = request.Description;
                    dto.QuestionsJsonStringArray =
                        JsonConvert.SerializeObject(request.UpsertChoiceOfAnswer.Questions);
                    dto.AnswersJsonBoolArray =
                        JsonConvert.SerializeObject(request.UpsertChoiceOfAnswer.Answers);
                }),
            
            
            InteractiveType.ProgramWriting => await _interactiveRepository.UpsertInteractiveAsync(
                dto => dto.Id == request.InteractiveId,
                () => new ProgramWritingDto
                    {
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,

                        Description = request.Description,
                        Code = request.UpsertProgramWriting.Code,
                        Output = request.UpsertProgramWriting.Output
                    },
                dto =>
                {
                    dto.Description = request.Description;
                    dto.Code = request.UpsertProgramWriting.Code;
                    dto.Output = request.UpsertProgramWriting.Output;
                }),
            
            
            InteractiveType.WritingOfAnswer => await _interactiveRepository.UpsertInteractiveAsync(
                dto => dto.Id == request.InteractiveId,
                () => new WritingOfAnswerDto
                    {
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,

                        Description = request.Description,
                        Answer = request.UpsertWritingOfAnswer.Answer
                    },
                dto =>
                {
                    dto.Description = request.Description;
                    dto.Answer = request.UpsertWritingOfAnswer.Answer;
                }),
            _ => new OperationResult<InteractivePublic>(new ArgumentException("Invalid interactive type."))
        };
        if (!interactiveOperation.Success)
            return new(interactiveOperation);

        return new(new InteractiveByIdPublic(interactiveOperation.Value));
    }


    public async Task<OperationResult<LastInteractiveByIdPublic>> UpsertInteractiveReplyAsync(
        UpsertInteractiveReplyRequest request, User user)
    {
        var articleOperation = await _articleRepository.GetArticleByIdAsync(request.CourseId, request.ArticleId);
        if (!articleOperation.Success)
            return new(articleOperation);

        var interactive = GetInteractiveReply(request);
        return interactive switch
        {
            InteractiveType.CheckList => await UpsertLastInteractiveAsync<CheckListDto, LastCheckListDto>
            (request.InteractiveId, 
                _ => new LastInteractiveByIdPublic(new LastCheckListPublic(request)),
                (lastInteractive)=> _interactiveRepository.UpsertInteractiveAsync(
                    dto => dto.UserId == user.Id && dto.InteractiveId == lastInteractive.LastCheckList.InteractiveId,
                    () => new LastCheckListDto
                    {
                        InteractiveId = request.InteractiveId,
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,
                        UserId = user.Id,
                        CheckedClausesJsonBoolArray = JsonConvert.SerializeObject(lastInteractive.LastCheckList.Clauses),
                    },
                    dto =>
                    {
                        dto.CheckedClausesJsonBoolArray = JsonConvert.SerializeObject(lastInteractive.LastCheckList.Clauses);
                    })),
            
            
            InteractiveType.ChoiceOfAnswer=>await UpsertLastInteractiveAsync<ChoiceOfAnswerDto, LastChoiceOfAnswerDto>
            (request.InteractiveId, 
                interactiveDto => new LastInteractiveByIdPublic(new LastChoiceOfAnswerPublic(request, interactiveDto)),
                (lastInteractive)=> _interactiveRepository.UpsertInteractiveAsync(
                    dto => dto.UserId == user.Id && dto.InteractiveId == lastInteractive.LastChoiceOfAnswer.InteractiveId,
                    () => new LastChoiceOfAnswerDto
                    {
                        InteractiveId = request.InteractiveId,
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,
                        UserId = user.Id,
                        SuccessAnswersJsonBoolArray = JsonConvert.SerializeObject(lastInteractive.LastChoiceOfAnswer.SuccessAnswers),
                        AnswersJsonBoolArray = JsonConvert.SerializeObject(lastInteractive.LastChoiceOfAnswer.Answers),
                    },
                    dto =>
                    {
                        dto.SuccessAnswersJsonBoolArray = JsonConvert.SerializeObject(lastInteractive.LastChoiceOfAnswer.SuccessAnswers);
                        dto.AnswersJsonBoolArray = JsonConvert.SerializeObject(lastInteractive.LastChoiceOfAnswer.Answers);
                    }).InvokeOnSuccessAsync(LogChoiceOfAnswerAsync)),
            
            
            InteractiveType.ProgramWriting=> await UpsertLastInteractiveAsync<ProgramWritingDto, LastProgramWritingDto>
            (request.InteractiveId, 
                interactiveDto => new LastInteractiveByIdPublic(new LastProgramWritingPublic(request, interactiveDto)),
                (lastInteractive)=> _interactiveRepository.UpsertInteractiveAsync(
                    dto => dto.UserId == user.Id && dto.InteractiveId == lastInteractive.LastProgramWriting.InteractiveId,
                    () => new LastProgramWritingDto
                    {
                        InteractiveId = request.InteractiveId,
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,
                        UserId = user.Id,
                        Code = request.UpsertReplyProgramWriting.Code,
                        Success = lastInteractive.LastProgramWriting.Success
                    },
                    dto =>
                    {
                        dto.Code = request.UpsertReplyProgramWriting.Code;
                        dto.Success = lastInteractive.LastProgramWriting.Success;
                    }).InvokeOnSuccessAsync(LogProgramWritingAsync)),
            
            
            InteractiveType.WritingOfAnswer=> await UpsertLastInteractiveAsync<WritingOfAnswerDto, LastWritingOfAnswerDto>
            (request.InteractiveId, 
                interactiveDto => new LastInteractiveByIdPublic(new LastWritingOfAnswerPublic(request, interactiveDto)),
                (lastInteractive)=> _interactiveRepository.UpsertInteractiveAsync(
                    dto => dto.UserId == user.Id && dto.InteractiveId == lastInteractive.LastWritingOfAnswer.InteractiveId,
                    () => new LastWritingOfAnswerDto
                    {
                        InteractiveId = request.InteractiveId,
                        ArticleId = request.ArticleId,
                        CourseId = request.CourseId,
                        UserId = user.Id,
                        Answer = request.UpsertReplyWritingOfAnswer.Answer,
                        Success = lastInteractive.LastWritingOfAnswer.Success
                    },
                    dto =>
                    {
                        dto.Answer = request.UpsertReplyWritingOfAnswer.Answer;
                        dto.Success = lastInteractive.LastWritingOfAnswer.Success;
                    }).InvokeOnSuccessAsync(LogWritingOfAnswerAsync)),
            
            _ => new (new ArgumentException("Invalid interactive type."))
        };
    }

    public async Task<OperationResult<InteractiveByIdPublic>> DeleteInteractiveAsync(InteractiveType interactiveType, int interactiveId)
    {
        return interactiveType switch
        {
            InteractiveType.CheckList => await _interactiveRepository.DeleteInteractiveByIdAsync<CheckListDto>(interactiveId),
            InteractiveType.ChoiceOfAnswer => await _interactiveRepository.DeleteInteractiveByIdAsync<ChoiceOfAnswerDto>(interactiveId),
            InteractiveType.ProgramWriting => await _interactiveRepository.DeleteInteractiveByIdAsync<ProgramWritingDto>(interactiveId),
            InteractiveType.WritingOfAnswer => await _interactiveRepository.DeleteInteractiveByIdAsync<WritingOfAnswerDto>(interactiveId),
            _ => new (new ArgumentException("Invalid interactive type."))
        };
    }

    private InteractiveType GetInteractive(UpsertInteractiveRequest request)
    {
        if (request.UpsertCheckList != null)
            return InteractiveType.CheckList;
        if (request.UpsertChoiceOfAnswer != null)
            return InteractiveType.ChoiceOfAnswer;
        if (request.UpsertProgramWriting != null)
            return InteractiveType.ProgramWriting;
        if (request.UpsertWritingOfAnswer != null)
            return InteractiveType.WritingOfAnswer;
        
        throw new NotImplementedException("Interactive body not implemented");
    }
    
    private InteractiveType GetInteractiveReply(UpsertInteractiveReplyRequest request)
    {
        if (request.UpsertReplyCheckList != null)
            return InteractiveType.CheckList;
        if (request.UpsertReplyAnswerChoice != null)
            return InteractiveType.ChoiceOfAnswer;
        if (request.UpsertReplyProgramWriting != null)
            return InteractiveType.ProgramWriting;
        if (request.UpsertReplyWritingOfAnswer != null)
            return InteractiveType.WritingOfAnswer;
        
        throw new NotImplementedException("Interactive body not implemented");
    }

    /// <summary>
    /// Такая сложная структура, потому что необходимо вернуть именно lastInteractive, а не lastInteractiveDto
    /// не производя лишних сериализаций
    /// </summary>
    /// <param name="interactiveId"></param>
    /// <param name="getLastInteractiveFunc"></param>
    /// <param name="upsertFunc"></param>
    /// <typeparam name="TInteractiveDto"></typeparam>
    /// <typeparam name="TLastInteractiveDto"></typeparam>
    /// <returns></returns>
    private async Task<OperationResult<LastInteractiveByIdPublic>> UpsertLastInteractiveAsync<TInteractiveDto,TLastInteractiveDto>(
        int interactiveId,
        Func<TInteractiveDto,  LastInteractiveByIdPublic> getLastInteractiveFunc,
        Func<LastInteractiveByIdPublic, Task<OperationResult<TLastInteractiveDto>>> upsertFunc)
    where TInteractiveDto: class, IHaveId
    {
        var interactiveOperation = await _interactiveRepository.GetInteractiveByIdAsync<TInteractiveDto>(interactiveId);
            if(!interactiveOperation.Success)
                return new(interactiveOperation);
        
        var lastInteractive = getLastInteractiveFunc(interactiveOperation.Value);

        OperationResult<LastInteractiveByIdPublic> lastInteractiveByIdPublic = null;
        await upsertFunc(lastInteractive)
            .InvokeOnErrorAsync(operationResult=>lastInteractiveByIdPublic = new (operationResult))
            .InvokeOnSuccessAsync(_=>lastInteractiveByIdPublic = new (lastInteractive));

        return lastInteractiveByIdPublic ?? new OperationResult<LastInteractiveByIdPublic>();
    }
    
     void LogChoiceOfAnswerAsync(LastChoiceOfAnswerDto dto) =>
         _interactiveRepository.InsertInteractiveAsync(() => new LogChoiceOfAnswerDto
        {
            InteractiveId = dto.InteractiveId,
            UserId = dto.UserId,
            LogDate = DateTimeOffset.Now,
            SuccessAnswersJsonBoolArray = dto.SuccessAnswersJsonBoolArray,
            AnswersJsonBoolArray = dto.AnswersJsonBoolArray
        }).Forget();
     
     void LogProgramWritingAsync(LastProgramWritingDto dto) =>
         _interactiveRepository.InsertInteractiveAsync(() => new LogProgramWritingDto()
         {
             InteractiveId = dto.InteractiveId,
             UserId = dto.UserId,
             LogDate = DateTimeOffset.Now,
             Code = dto.Code,
             Success = dto.Success
         }).Forget();
     
     void LogWritingOfAnswerAsync(LastWritingOfAnswerDto dto) =>
         _interactiveRepository.InsertInteractiveAsync(() => new LogWritingOfAnswerDto()
         {
             InteractiveId = dto.InteractiveId,
             UserId = dto.UserId,
             LogDate = DateTimeOffset.Now,
             Success = dto.Success,
             Answer = dto.Answer
         }).Forget();
}