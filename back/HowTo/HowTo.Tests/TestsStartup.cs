using HowTo.DataAccess.Helpers;
using HowTo.DataAccess.Managers;
using HowTo.DataAccess.Repositories;
using HowTo.Entities.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HowTo.Tests;

public class TestsStartup<TestClassName>
{
    public ArticleManager ArticleManager { get; set; }
    public CourseManager CourseManager { get; set; }
    public ViewManager ViewManager{ get; set; }
    public UserInfoManager UserInfoManager{ get; set; }
    public FileSystemHelper FileSystemHelper{ get; set; }
    public SummaryManager SummaryManager{ get; set; }
    public InteractiveManager InteractiveManager{ get; set; }
    public IDbContextFactory<ApplicationContext> DbContextFactory{ get; set; }
    public readonly string RootPath = $"/Users/gently/Temp/{typeof(TestClassName).FullName}-howto-test-content/{Guid.NewGuid()}/";
    public TestsStartup()
    {
        var provider = SetUpProvider();
        ArticleManager = provider.GetService<ArticleManager>();
        CourseManager = provider.GetService<CourseManager>();
        ViewManager = provider.GetService<ViewManager>();
        UserInfoManager = provider.GetService<UserInfoManager>();
        FileSystemHelper = provider.GetService<FileSystemHelper>();
        SummaryManager = provider.GetService<SummaryManager>();
        InteractiveManager = provider.GetService<InteractiveManager>();
        DbContextFactory = provider.GetService<IDbContextFactory<ApplicationContext>>();
        FileSystemHelper = provider.GetService<FileSystemHelper>();
    }
    public IServiceProvider SetUpProvider()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContextFactory<ApplicationContext>((options) => { options.UseSqlite(); });
        serviceCollection.WithRepositories();
        serviceCollection.WithMangers();

        serviceCollection.AddSingleton(new FileSystemHelper(Options.Create(new FileSystemOptions
        {
            RootPath = RootPath
        })));
        Directory.CreateDirectory(RootPath);
        serviceCollection.AddSingleton(Options.Create(new DbSettings()
        {
            DefaultConnection = $"Data Source={RootPath}/{Guid.NewGuid()}"
        }));

        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        using (var scope = serviceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetService<ApplicationContext>();
            context.Database.EnsureCreated();
        }
        
        return serviceProvider;
    }
}