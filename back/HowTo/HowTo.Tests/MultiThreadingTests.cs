using ATI.Services.Common.Behaviors;
using HowTo.Entities;
using HowTo.Entities.Article;
using HowTo.Entities.Course;

namespace HowTo.Tests;

public class MultiThreadingTests : BaseTests<MultiThreadingTests>
{
    [Fact]
    public async void  MultiUpsertingAsync()
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

        var courseTaskList = new List<Task<OperationResult<CoursePublic>>>(100);
        var articleTaskList = new List<Task<OperationResult<ArticlePublic>>>(100);
        foreach (var id in Enumerable.Range(1, 100))
        {
            var article = new UpsertArticleRequest
            {
                CourseId = courseOperation.Value.Id,
                Title = "TestArticleTitle",
                File = GetFormFile(_firstFormFileContent)
            };
            var course = new UpsertCourseRequest
            {
                Title = id.ToString(),
                Description = "TestCourseDescription",
                File = GetFormImage()
            };
            courseTaskList.Add(Startup.CourseManager.UpsertCourseAsync(course, user));
            articleTaskList.Add(Startup.ArticleManager.UpsertArticleAsync(article, user));
        }

        await Task.WhenAll(courseTaskList);
        await Task.WhenAll(articleTaskList);

        foreach (var task in courseTaskList)
        {
            Assert.True(task.Result.Success, task.Result.DumpAllErrors());
        }
        foreach (var task in articleTaskList)
        {
            Assert.True(task.Result.Success, task.Result.DumpAllErrors());
        }
    }
}