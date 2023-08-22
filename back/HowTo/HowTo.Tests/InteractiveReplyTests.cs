using ATI.Services.Common.Extensions.OperationResult;
using HowTo.Entities.Interactive.CheckList;
using HowTo.Entities.Interactive.ChoiceOfAnswers;
using HowTo.Entities.Interactive.ProgramWriting;
using HowTo.Entities.Interactive.WritingOfAnswer;

namespace HowTo.Tests;

public class InteractiveReplyTests : BaseTestsWithArtefacts<InteractiveReplyTests>
{
    [Fact]
    private async void CreateAllInteractiveAndReplyAsync()
    {
        var courseOperation = await InitCourseAsync();
        var articleOperation = await InitArticleAsync(courseOperation.Value);
        var checkListOperation = await InitInteractiveAsync(articleOperation.Value,  checkListRequest: new UpsertCheckListRequest()
        {
            Clauses = new [] {"test1", "test2", "test3"}
        });
        var choiceOfAnswerOperation = await InitInteractiveAsync(articleOperation.Value,  choiceOfAnswerRequest: new UpsertChoiceOfAnswerRequest()
        {
            
            Questions = new [] {"test1", "test2", "test3"},
            Answers = new [] {true, false, false}
        });
        var programWritingOperation = await InitInteractiveAsync(articleOperation.Value, programWritingRequest:new UpsertProgramWritingRequest()
        {
            Code = "<div>success</div>",
        });
        var writingOfAnswerOperation = await InitInteractiveAsync(articleOperation.Value, writingOfAnswerRequest:new UpsertWritingOfAnswerRequest
        {
            Answer = "hello world"
        });

        var checkListReply = new UpsertReplyCheckListRequest()
        {
            Clauses = new bool?[] { true, false, true }
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,checkListOperation.Value.CheckList.Id,
            checkListRequest: checkListReply);

        var choiceOfAnswerReply = new UpsertReplyAnswerChoiceRequest
        {
            Answers = new[] { false, false, true }
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,choiceOfAnswerOperation.Value.ChoiceOfAnswer.Id,
            choiceOfAnswerRequest: choiceOfAnswerReply);

        var programWritingReply = new UpsertReplyProgramWritingRequest
        {
            Code = "codeuser",
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,programWritingOperation.Value.ProgramWriting.Id,
            programWritingRequest: programWritingReply);

        var writingOfAnswerReply = new UpsertReplyWritingOfAnswerRequest
        {
            Answer = "answer from user"
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,writingOfAnswerOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: writingOfAnswerReply);

        var interactiveOperation = await 
            Startup.InteractiveManager.GetInteractiveAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
                FirstUser).InvokeOnErrorAsync(operationResult => Assert.Fail(operationResult.DumpAllErrors()));
        Assert.True(interactiveOperation.Value.CheckList[0].UserClausesChecked.SequenceEqual(checkListReply.Clauses));
        Assert.True(interactiveOperation.Value.ChoiceOfAnswer[0].UserAnswers.SequenceEqual(choiceOfAnswerReply.Answers));
        Assert.True(interactiveOperation.Value.ProgramWriting[0].UserCode.Equals(programWritingReply.Code));
        Assert.True(interactiveOperation.Value.WritingOfAnswer[0].UserAnswer.Equals(writingOfAnswerReply.Answer));
    }
    
    [Fact]
    private async void CreateCheckListInteractiveAndReplyAsync()
    {
        var courseOperation = await InitCourseAsync();
        var articleOperation = await InitArticleAsync(courseOperation.Value);
        var request = new UpsertCheckListRequest
        {
            Clauses = new[] { "test1", "test2", "test3" }
        };
        var initInteractiveOperation = await InitInteractiveAsync(articleOperation.Value, 
            checkListRequest: request);
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,initInteractiveOperation.Value.CheckList.Id,
            checkListRequest: new UpsertReplyCheckListRequest
            {
                Clauses = new bool?[] {true,false, true}
            });
        
        var lastReply = new UpsertReplyCheckListRequest
        {
            Clauses = new bool?[] {true ,false, true}
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
            initInteractiveOperation.Value.CheckList.Id,
            checkListRequest: lastReply);
        
        var interactiveOperation = await 
            Startup.InteractiveManager.GetInteractiveAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
                FirstUser).InvokeOnErrorAsync(operationResult => Assert.Fail(operationResult.DumpAllErrors()));
        
        Assert.True(lastReply.Clauses.SequenceEqual(interactiveOperation.Value.CheckList[0].UserClausesChecked));
    }
    
    [Fact]
    private async void CreateChoiceOfAnswerInteractiveAndReplyAsync()
    {
        var courseOperation = await InitCourseAsync();
        var articleOperation = await InitArticleAsync(courseOperation.Value);
        var request = new UpsertChoiceOfAnswerRequest
        {
            Answers = new[] { true, true, false },
            Questions = new[] { "test1", "test2", "test3" }
        };
    
        var initInteractiveOperation = await InitInteractiveAsync(articleOperation.Value, 
            choiceOfAnswerRequest: request);
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,initInteractiveOperation.Value.ChoiceOfAnswer.Id,
            choiceOfAnswerRequest: new UpsertReplyAnswerChoiceRequest
            {
                Answers = new [] {false ,false, false}
            });
        
        var lastReply = new UpsertReplyAnswerChoiceRequest
        {
            Answers = new [] {true ,true, false}
        };
        var successReply = new UpsertReplyAnswerChoiceRequest
        {
            Answers = new [] {true ,true, true}
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
            initInteractiveOperation.Value.ChoiceOfAnswer.Id,
            choiceOfAnswerRequest: lastReply);
        
        var interactiveOperation = await 
            Startup.InteractiveManager.GetInteractiveAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
                FirstUser).InvokeOnErrorAsync(operationResult => Assert.Fail(operationResult.DumpAllErrors()));
        
        Assert.True(lastReply.Answers.SequenceEqual(interactiveOperation.Value.ChoiceOfAnswer[0].UserAnswers));
        Assert.True(successReply.Answers.SequenceEqual(interactiveOperation.Value.ChoiceOfAnswer[0].UserSuccessAnswers));
    }
    
    [Fact]
    private async void CreateProgramWritingInteractiveReplyAsync()
    {
        var courseOperation = await InitCourseAsync();
        var articleOperation = await InitArticleAsync(courseOperation.Value);
        var request = new UpsertProgramWritingRequest
        {
            Code = "code",
        };
        var initInteractiveOperation = await InitInteractiveAsync(articleOperation.Value, 
            programWritingRequest: request);
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,initInteractiveOperation.Value.ProgramWriting.Id,
            programWritingRequest: new UpsertReplyProgramWritingRequest
            {
                Code = "false"
            });
        
        var lastReply = new UpsertReplyProgramWritingRequest
        {
            Code = "success"
        };
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
            initInteractiveOperation.Value.ProgramWriting.Id,
            programWritingRequest: lastReply);
        
        var interactiveOperation = await 
            Startup.InteractiveManager.GetInteractiveAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
                FirstUser).InvokeOnErrorAsync(operationResult => Assert.Fail(operationResult.DumpAllErrors()));
        Assert.Equal(lastReply.Code, interactiveOperation.Value.ProgramWriting[0].UserCode);
        Assert.True(interactiveOperation.Value.ProgramWriting[0].UserSuccess);
    }
    
    [Fact]
    private async void CreateWritingOfAnswerInteractiveReplyAsync()
    {
        var courseOperation = await InitCourseAsync();
        var articleOperation = await InitArticleAsync(courseOperation.Value);
        const string description = "interactiveDescription";
        var request = new UpsertWritingOfAnswerRequest
        {
            Answer = "success",
        };
        var initInteractiveOperation = await InitInteractiveAsync(articleOperation.Value, 
            writingOfAnswerRequest: request, description: description);
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,initInteractiveOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: new UpsertReplyWritingOfAnswerRequest()
            {
                Answer = "bad"
            });
        
        var lastReply = new UpsertReplyWritingOfAnswerRequest()
        {
            Answer = "success"
        };
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
            initInteractiveOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: lastReply);
        
        var interactiveOperation = await 
            Startup.InteractiveManager.GetInteractiveAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
                FirstUser).InvokeOnErrorAsync(operationResult => Assert.Fail(operationResult.DumpAllErrors()));

        Assert.Equal(lastReply.Answer, interactiveOperation.Value.WritingOfAnswer[0].UserAnswer);
        Assert.True(interactiveOperation.Value.WritingOfAnswer[0].UserSuccess);
    }
    
    [Fact]
    private async void CreateSeveralInteractiveRepliesAsync()
    {
        var courseOperation = await InitCourseAsync();
        var articleOperation = await InitArticleAsync(courseOperation.Value);
        var request = new UpsertWritingOfAnswerRequest
        {
            Answer = "success",
        };
        var initFirstInteractiveOperation = await InitInteractiveAsync(articleOperation.Value, 
            writingOfAnswerRequest: request);
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,initFirstInteractiveOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: new UpsertReplyWritingOfAnswerRequest()
            {
                Answer = "bad"
            });
        
        var firstLastReply = new UpsertReplyWritingOfAnswerRequest()
        {
            Answer = "success"
        };
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
            initFirstInteractiveOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: firstLastReply);
        
        var initSecondInteractiveOperation = await InitInteractiveAsync(articleOperation.Value, 
            writingOfAnswerRequest: request);
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId,articleOperation.Value.Id,initSecondInteractiveOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: new UpsertReplyWritingOfAnswerRequest()
            {
                Answer = "bad"
            });
        
        var secondLastReply = new UpsertReplyWritingOfAnswerRequest()
        {
            Answer = "worse"
        };
        
        await InitInteractiveReplyAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
            initSecondInteractiveOperation.Value.WritingOfAnswer.Id,
            writingOfAnswerRequest: secondLastReply);
        
        var interactiveOperation = await 
            Startup.InteractiveManager.GetInteractiveAsync(articleOperation.Value.CourseId, articleOperation.Value.Id,
                FirstUser).InvokeOnErrorAsync(operationResult => Assert.Fail(operationResult.DumpAllErrors()));

        Assert.Equal(firstLastReply.Answer, interactiveOperation.Value.WritingOfAnswer[0].UserAnswer);
        Assert.True(interactiveOperation.Value.WritingOfAnswer[0].UserSuccess);
        
        Assert.Equal(secondLastReply.Answer, interactiveOperation.Value.WritingOfAnswer[1].UserAnswer);
        Assert.False(interactiveOperation.Value.WritingOfAnswer[1].UserSuccess);
    }
}