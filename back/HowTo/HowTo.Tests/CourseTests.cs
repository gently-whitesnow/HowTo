using System.Text;
using ATI.Services.Common.Behaviors;
using ATI.Services.Common.Extensions.OperationResult;
using HowTo.Entities;
using HowTo.Entities.Article;
using HowTo.Entities.Course;
using Microsoft.EntityFrameworkCore;

namespace HowTo.Tests;

public class CourseTests : BaseTestsWithArtefacts<CourseTests>
{
    [Fact]
    private async void CreateCourseAsync()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "TestUserName", UserRole.None);
        var courseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitle",
            Description = "TestCourseDescription",
            File = GetFormImage()
        };
        await Startup.CourseManager.UpsertCourseAsync(courseRequest, user)
            .InvokeOnSuccessAsync(course => Startup.CourseManager.GetCourseWithFilesByIdAsync(course.Id, user)
                .InvokeOnSuccessAsync(coursePublic =>
                {
                    Assert.Equal(courseRequest.Title, coursePublic.Title);
                    Assert.Equal(courseRequest.Description, coursePublic.Description);
                    Assert.Single(coursePublic.Files);
                })
                .InvokeOnErrorAsync(operation => Assert.Fail(operation.DumpAllErrors()))
            )
            .InvokeOnErrorAsync(operation => Assert.Fail(operation.DumpAllErrors()));
    }

    [Fact]
    private async void UpdateCourseAsync()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "TestUserName", UserRole.None);
        var courseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitle",
            Description = "TestCourseDescription",
            File = GetFormImage()
        };
        var courseOperation = await Startup.CourseManager.UpsertCourseAsync(courseRequest, user);
        Assert.True(courseOperation.Success);

        var image = GetFormImage();
        var updateCourseRequest = new UpsertCourseRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestCourseTitleSecond",
            Description = "TestCourseDescriptionSecond",
            File = image
        };
        var updateCourseOperation = await Startup.CourseManager.UpsertCourseAsync(updateCourseRequest, user);
        Assert.True(updateCourseOperation.Success);

        var getFileOperation =
            await Startup.FileSystemHelper.GetCourseFilesAsync(courseOperation.Value.Id);
        Assert.True(getFileOperation.Success);
        Assert.Single(getFileOperation.Value);
        Assert.False(CompareByteArrayAndFormFile(getFileOperation.Value.First(), image));

        await using var db = await Startup.DbContextFactory.CreateDbContextAsync();
        var courseDto = await db.CourseContext.SingleOrDefaultAsync
        (c => c.Id == updateCourseOperation.Value.Id
              && c.Title == updateCourseRequest.Title
              && c.Description == updateCourseRequest.Description);

        Assert.NotNull(courseDto);
    }

    [Fact]
    private async void DeleteCourseAsync()
    {
        var userId = Guid.NewGuid();

        var user = new User(userId, "TestUserName", UserRole.None);
        var courseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitle",
            Description = "TestCourseDescription",
            File = GetFormImage()
        };
        var upsertCourseOperation = await Startup.CourseManager.UpsertCourseAsync(courseRequest, user);
        Assert.True(upsertCourseOperation.Success, upsertCourseOperation.DumpAllErrors());

        var getCourseOperation =
            await Startup.CourseManager.GetCourseWithFilesByIdAsync(upsertCourseOperation.Value.Id, user);
        Assert.True(getCourseOperation.Success, getCourseOperation.DumpAllErrors());

        var deleteOperation = await Startup.CourseManager.DeleteCourseAsync(getCourseOperation.Value.Id);
        Assert.True(deleteOperation.Success, deleteOperation.DumpAllErrors());

        var getFileAfterDeleteOperation = await Startup.FileSystemHelper.GetCourseFilesAsync(
            deleteOperation.Value.Id);
        Assert.Equal(ActionStatus.Ok, getFileAfterDeleteOperation.ActionStatus);
        Assert.False(getFileAfterDeleteOperation.Success);
    }

    [Fact]
    private async void CheckCourseAuthorsAsync()
    {
        var firstUserId = Guid.NewGuid();
        var secondUserId = Guid.NewGuid();
        var firstUser = new User(firstUserId, "FirstTestUserName", UserRole.None);
        var secondUser = new User(secondUserId, "SecondTestUserName", UserRole.None);
        var courseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitle",
            Description = "TestCourseDescription"
        };
        var courseOperation = await Startup.CourseManager.UpsertCourseAsync(courseRequest, firstUser);
        Assert.True(courseOperation.Success, courseOperation.DumpAllErrors());

        var articleRequest = new UpsertArticleRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        };

        var firstArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, firstUser);
        Assert.True(firstArticleOperation.Success, firstArticleOperation.DumpAllErrors());
        var secondArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, secondUser);
        Assert.True(secondArticleOperation.Success, secondArticleOperation.DumpAllErrors());

        var getCourseOperation =
            await Startup.CourseManager.GetCourseWithFilesByIdAsync(courseOperation.Value.Id, firstUser);
        Assert.True(getCourseOperation.Success, secondArticleOperation.DumpAllErrors());
        Assert.Equal(2, getCourseOperation.Value.Contributors.Count());
    }

    [Fact]
    private async void CreateAndUpdateStatusArticleAsync()
    {
        var courseOperation = await InitCourseAsync(user: FirstUser);

        Assert.Equal(courseOperation.Value.Status, EntityStatus.Moderation);

        var updateStatusOperation = await Startup.CourseManager.UpdateStatusCourseAsync(new UpdateStatusCourseRequest
        {
            CourseId = courseOperation.Value.Id,
            Status = EntityStatus.Published
        });
        Assert.True(updateStatusOperation.Success, updateStatusOperation.DumpAllErrors());
        Assert.Equal(updateStatusOperation.Value.Status, EntityStatus.Published);

        var getAfterUpdateCourseOperation =
            await Startup.CourseManager.GetCourseWithFilesByIdAsync(updateStatusOperation.Value.Id, FirstUser);
        Assert.True(getAfterUpdateCourseOperation.Success, getAfterUpdateCourseOperation.DumpAllErrors());
        Assert.Equal(getAfterUpdateCourseOperation.Value.Status, EntityStatus.Published);
    }
    
    [Fact]
    public async void CheckCourseViewConditionAsync()
    {
        var firstCourseOperation = await InitCourseAsync(user: FirstUser, courseTitle:"firstTitle");
        await InitArticleAsync(firstCourseOperation.Value, user: FirstUser);
        var secondArticleOperation = await InitArticleAsync(firstCourseOperation.Value, user: FirstUser);
        
        var courseByAuthorOperation = await Startup.CourseManager.GetCourseWithFilesByIdAsync(firstCourseOperation.Value.Id, FirstUser);
        Assert.True(courseByAuthorOperation.Success, courseByAuthorOperation.DumpAllErrors());
        Assert.Equal(2, courseByAuthorOperation.Value.Articles.Count());

        var courseByAdminOperation = await Startup.CourseManager.GetCourseWithFilesByIdAsync(firstCourseOperation.Value.Id, AdminUser);
        Assert.True(courseByAdminOperation.Success, courseByAuthorOperation.DumpAllErrors());
        Assert.Equal(2, courseByAdminOperation.Value.Articles.Count());
        
        var updateStatusOperation = await Startup.ArticleManager.UpdateStatusArticleAsync(new UpdateStatusArticleRequest
        {
            CourseId = secondArticleOperation.Value.CourseId,
            ArticleId= secondArticleOperation.Value.Id,
            Status = EntityStatus.Published
        });
        Assert.True(updateStatusOperation.Success, updateStatusOperation.DumpAllErrors());
        
        var courseByAnotherUserOperation = await Startup.CourseManager.GetCourseWithFilesByIdAsync(firstCourseOperation.Value.Id, SecondUser);
        Assert.True(courseByAnotherUserOperation.Success, courseByAnotherUserOperation.DumpAllErrors());
        Assert.Equal(1, courseByAnotherUserOperation.Value.Articles.Count());
    }
}