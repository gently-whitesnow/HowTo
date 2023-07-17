using System.Text;
using ATI.Services.Common.Behaviors;
using HowTo.Entities;
using HowTo.Entities.Article;
using HowTo.Entities.Course;
using Microsoft.EntityFrameworkCore;

namespace HowTo.Tests;

public class ArticleTests : BaseTestsWithArtefacts<ArticleTests>
{
    [Fact]
    private async void CreateArticleAsync()
    {
        var courseOperation = await InitCourseAsync(user: FirstUser);

        var articleRequest = new UpsertArticleRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        };

        var articleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, FirstUser);
        Assert.True(articleOperation.Success);

        var getFileOperation =
            await Startup.FileSystemHelper.GetArticleFilesAsync(articleRequest.CourseId, articleOperation.Value.Id);
        Assert.True(getFileOperation.Success);
        Assert.Single(getFileOperation.Value);

        await using var db = await Startup.DbContextFactory.CreateDbContextAsync();
        var articleDto = await db.ArticleContext.SingleOrDefaultAsync
        (a => a.CourseId == articleRequest.CourseId
              && a.Id == articleOperation.Value.Id
              && a.Title == articleRequest.Title
              && a.Author.UserId == FirstUser.Id);

        Assert.NotNull(articleDto);
    }

    [Fact]
    private async void UpdateArticleAsync()
    {
        var courseOperation = await InitCourseAsync(user: FirstUser);

        var insertArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(new UpsertArticleRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        }, FirstUser);
        Assert.True(insertArticleOperation.Success);

        var updateArticleRequest = new UpsertArticleRequest
        {
            ArticleId = insertArticleOperation.Value.Id,
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleSecondTitle",
            File = GetFormFile(_secondFormFileContent)
        };
        var updateArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(updateArticleRequest, FirstUser);
        Assert.True(updateArticleOperation.Success);

        var getFileOperation =
            await Startup.FileSystemHelper.GetArticleFilesAsync(courseOperation.Value.Id, updateArticleOperation.Value.Id);
        Assert.True(getFileOperation.Success);
        Assert.Single(getFileOperation.Value);
        Assert.Equal(_secondFormFileContent, Encoding.UTF8.GetString(getFileOperation.Value.First()));

        await using var db = await Startup.DbContextFactory.CreateDbContextAsync();
        var articleDto = await db.ArticleContext.SingleOrDefaultAsync
        (a => a.CourseId == courseOperation.Value.Id
              && a.Id == insertArticleOperation.Value.Id
              && a.Title == updateArticleRequest.Title);

        Assert.NotNull(articleDto);
    }

    [Fact]
    private async void GetArticleAsync()
    {
        await InitCourseAsync(user: FirstUser, courseTitle: "FirstCourseTitle");
        
        var forCheckingLastCourseOperation = await InitCourseAsync(user: FirstUser, courseTitle: "SecondCourseTitle");

        var articleRequest = new UpsertArticleRequest
        {
            CourseId = forCheckingLastCourseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        };

        var upsertArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, FirstUser);
        Assert.True(upsertArticleOperation.Success, upsertArticleOperation.DumpAllErrors());
        var requesterUserId = Guid.NewGuid();
        var requesterUser = new User(requesterUserId, "RequesterTestUserName");
        var getArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(upsertArticleOperation.Value.CourseId,
                upsertArticleOperation.Value.Id, requesterUser);
        Assert.True(getArticleOperation.Success, getArticleOperation.DumpAllErrors());
        var summaryOperation = await Startup.SummaryManager.GetSummaryAsync(requesterUser);
        Assert.True(summaryOperation.Success, summaryOperation.DumpAllErrors());

        Assert.Equal(upsertArticleOperation.Value.CourseId, summaryOperation.Value.LastCourse?.Id);
    }

    [Fact]
    private async void DeleteArticleAfterDeleteCourseAsync()
    {
        var courseOperation = await InitCourseAsync(user: FirstUser);
        
        var articleRequest = new UpsertArticleRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        };

        var firstArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, FirstUser);
        Assert.True(firstArticleOperation.Success, firstArticleOperation.DumpAllErrors());
        var secondArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, SecondUser);
        Assert.True(secondArticleOperation.Success, secondArticleOperation.DumpAllErrors());
        
        var firstGetBySecondArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(firstArticleOperation.Value.CourseId,
                firstArticleOperation.Value.Id, SecondUser);
        Assert.True(firstGetBySecondArticleOperation.Success, firstGetBySecondArticleOperation.DumpAllErrors());
        
        var secondGetByFirstArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(firstArticleOperation.Value.CourseId,
                firstArticleOperation.Value.Id, FirstUser);
        Assert.True(secondGetByFirstArticleOperation.Success, secondGetByFirstArticleOperation.DumpAllErrors());
        
        var deleteOperation = await Startup.CourseManager.DeleteCourseAsync(firstArticleOperation.Value.CourseId);
        Assert.True(deleteOperation.Success, deleteOperation.DumpAllErrors());
        
        var getAfterDeleteFirstArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(firstArticleOperation.Value.CourseId,
                firstArticleOperation.Value.Id, SecondUser);
        Assert.Equal(ActionStatus.BadRequest, getAfterDeleteFirstArticleOperation.ActionStatus);
        
        var getAfterDeleteSecondArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(firstArticleOperation.Value.CourseId,
                firstArticleOperation.Value.Id, FirstUser);
        Assert.Equal(ActionStatus.BadRequest, getAfterDeleteSecondArticleOperation.ActionStatus);

        var getFileAfterDeleteFirstArticleOperation = await Startup.FileSystemHelper.GetArticleFilesAsync(
            firstArticleOperation.Value.CourseId,
            firstArticleOperation.Value.Id);
        Assert.Equal(ActionStatus.Ok, getFileAfterDeleteFirstArticleOperation.ActionStatus);
        Assert.False(getFileAfterDeleteFirstArticleOperation.Success);
        
        var getFileAfterDeleteSecondArticleOperation = await Startup.FileSystemHelper.GetArticleFilesAsync(
            secondArticleOperation.Value.CourseId,
            secondArticleOperation.Value.Id);
        Assert.Equal(ActionStatus.Ok, getFileAfterDeleteSecondArticleOperation.ActionStatus);
        Assert.False(getFileAfterDeleteSecondArticleOperation.Success);
    }
    
    
    [Fact]
    private async void DeleteArticleAsync()
    {
        var courseOperation = await InitCourseAsync(user: FirstUser);
        
        var articleRequest = new UpsertArticleRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        };

        var upsertArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, FirstUser);
        Assert.True(upsertArticleOperation.Success, upsertArticleOperation.DumpAllErrors());
        
        var getArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(upsertArticleOperation.Value.CourseId,
                upsertArticleOperation.Value.Id, FirstUser);
        Assert.True(getArticleOperation.Success, getArticleOperation.DumpAllErrors());
        
        var deleteOperation = await Startup.ArticleManager.DeleteArticleAsync(getArticleOperation.Value.Article.CourseId, getArticleOperation.Value.Article.Id);
        Assert.True(deleteOperation.Success, deleteOperation.DumpAllErrors());
        
        var getAfterDeleteSecondArticleOperation =
            await Startup.ArticleManager.GetArticleWithFileByIdAsync(deleteOperation.Value.CourseId,
                deleteOperation.Value.Id, FirstUser);
        Assert.Equal(ActionStatus.BadRequest, getAfterDeleteSecondArticleOperation.ActionStatus);
        
        var getFileAfterDeleteFirstArticleOperation = await Startup.FileSystemHelper.GetArticleFilesAsync(
            deleteOperation.Value.CourseId,
            deleteOperation.Value.Id);
        Assert.Equal(ActionStatus.Ok, getFileAfterDeleteFirstArticleOperation.ActionStatus);
        Assert.False(getFileAfterDeleteFirstArticleOperation.Success);
    }
}