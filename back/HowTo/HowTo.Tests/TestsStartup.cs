using HowTo.DataAccess.Helpers;
using HowTo.DataAccess.Managers;
using HowTo.DataAccess.Repositories;
using HowTo.Entities.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HowTo.Tests;

public class TestsStartup<TestClassName> :IDisposable
{
    public ArticleManager ArticleManager { get; set; }
    public CourseManager CourseManager { get; set; }
    public ViewManager ViewManager{ get; set; }
    public UserInfoManager UserInfoManager{ get; set; }
    public FileSystemHelper FileSystemHelper{ get; set; }
    public SummaryManager SummaryManager{ get; set; }
    public InteractiveManager InteractiveManager{ get; set; }
    public IDbContextFactory<ApplicationContext> DbContextFactory{ get; set; }
    public readonly string RootPath = $"/Users/gently/Temp/{typeof(TestClassName).FullName}-howto-test-content";

    private ServiceProvider _serviceProvider;
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

        serviceCollection.AddSingleton<FileSystemHelper>(new FileSystemHelper(Options.Create(new FileSystemOptions
        {
            RootPath = RootPath
        })));
        serviceCollection.AddSingleton(Options.Create(new DbSettings()
        {
            DefaultConnection = $"Data Source={Guid.NewGuid()};Mode=Memory;Cache=Shared;"
        }));

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        
        using (var scope = scopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
            dbContext.Database.EnsureCreated();
        }

        _serviceProvider = serviceProvider;
        return serviceProvider;
    }


    public void Dispose()
    {
        var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
        if (scopeFactory is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}