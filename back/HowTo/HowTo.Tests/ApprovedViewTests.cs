using HowTo.Entities;
using HowTo.Entities.Article;
using HowTo.Entities.Course;
using HowTo.Entities.ViewedEntity;

namespace HowTo.Tests;

public class ApprovedViewTests : BaseTestsWithArtefacts<ApprovedViewTests>
{
    [Fact]
    public async void AddApprovedViewAsync()
    {
        var userId = Guid.NewGuid();
        var user = new User(userId, "TestUserName", UserRole.None);
        var courseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitle",
            Description = "TestCourseDescription"
        };
        var courseOperation = await Startup.CourseManager.UpsertCourseAsync(courseRequest, user);
        Assert.True(courseOperation.Success);

        var articleRequest = new UpsertArticleRequest
        {
            CourseId = courseOperation.Value.Id,
            Title = "TestArticleTitle",
            File = GetFormFile(_firstFormFileContent)
        };

        var firstArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, user);
        Assert.True(firstArticleOperation.Success);
        
        var secondArticleOperation = await Startup.ArticleManager.UpsertArticleAsync(articleRequest, user);
        Assert.True(secondArticleOperation.Success);

        var approvedViewOperation = await Startup.UserInfoManager.AddApprovedViewAsync(user, new AddApprovedViewRequest
        {
            CourseId = firstArticleOperation.Value.CourseId,
            ArticleId = firstArticleOperation.Value.Id
        });
        Assert.True(approvedViewOperation.Success);

        var summaryOperation = await Startup.SummaryManager.GetSummaryAsync(user);
        Assert.True(summaryOperation.Success);
        Assert.Equal(1, summaryOperation.Value.LastCourse?.UserApprovedViews);
        
        var getArticleOperation = await Startup.ArticleManager.GetArticleWithFileByIdAsync(
            firstArticleOperation.Value.CourseId,firstArticleOperation.Value.Id, user);
        Assert.True(getArticleOperation.Success, getArticleOperation.DumpAllErrors());
        
        Assert.True(getArticleOperation.Value.Article.IsAuthor);
        Assert.True(getArticleOperation.Value.Article.IsViewed);
    }
}