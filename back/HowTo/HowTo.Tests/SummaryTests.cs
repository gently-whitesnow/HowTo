using HowTo.Entities;
using HowTo.Entities.Course;

namespace HowTo.Tests;

public class SummaryTests : BaseTestsWithArtefacts<SummaryTests>
{
    [Fact]
    public async void CheckSummaryFilesAsync()
    {
        var firstCourseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitleFirst",
            Description = "TestCourseDescription",
            File = GetFormImage()
        };
        var firstCourseOperation = await Startup.CourseManager.UpsertCourseAsync(firstCourseRequest, FirstUser);
        Assert.True(firstCourseOperation.Success, firstCourseOperation.DumpAllErrors());

        var secondCourseRequest = new UpsertCourseRequest
        {
            Title = "TestCourseTitleSecond",
            Description = "TestCourseDescription",
            File = GetFormImage()
        };
        var secondCourseOperation = await Startup.CourseManager.UpsertCourseAsync(secondCourseRequest, FirstUser);
        Assert.True(secondCourseOperation.Success, firstCourseOperation.DumpAllErrors());

        var summaryOperation = await Startup.SummaryManager.GetSummaryAsync(FirstUser);
        Assert.True(summaryOperation.Success, firstCourseOperation.DumpAllErrors());
        Assert.Equal(2, summaryOperation.Value.Courses.Count(d => d.Files.Count() == 1));
    }
    
    [Fact]
    public async void CheckSummaryViewConditionAsync()
    {
        var firstCourseOperation = await InitCourseAsync(user: FirstUser, courseTitle:"firstTitle");
        await InitCourseAsync(user: FirstUser, courseTitle:"secondTitle");
        await InitArticleAsync(firstCourseOperation.Value, user: FirstUser);
        await InitArticleAsync(firstCourseOperation.Value, user: FirstUser);
        
        var summaryByAuthorOperation = await Startup.SummaryManager.GetSummaryAsync(FirstUser);
        Assert.True(summaryByAuthorOperation.Success, summaryByAuthorOperation.DumpAllErrors());
        Assert.Equal(2, summaryByAuthorOperation.Value.Courses.Count);

        var summaryByAdminOperation = await Startup.SummaryManager.GetSummaryAsync(AdminUser);
        Assert.True(summaryByAdminOperation.Success, summaryByAdminOperation.DumpAllErrors());
        Assert.Equal(2, summaryByAdminOperation.Value.Courses.Count);
        
        var updateOperation = await Startup.CourseManager.UpdateStatusCourseAsync(new UpdateStatusCourseRequest
        {
            CourseId = firstCourseOperation.Value.Id,
            Status = EntityStatus.Published
        });
        Assert.True(updateOperation.Success, updateOperation.DumpAllErrors());
        
        var summaryByAnotherUserOperation = await Startup.SummaryManager.GetSummaryAsync(SecondUser);
        Assert.True(summaryByAnotherUserOperation.Success, summaryByAnotherUserOperation.DumpAllErrors());
        Assert.Single(summaryByAnotherUserOperation.Value.Courses);
    }
}