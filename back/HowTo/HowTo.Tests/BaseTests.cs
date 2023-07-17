using System.Text;
using ATI.Services.Common.Extensions;
using HowTo.DataAccess.Helpers;
using HowTo.DataAccess.Managers;
using HowTo.DataAccess.Repositories;
using HowTo.Entities;
using HowTo.Entities.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace HowTo.Tests;

public abstract class BaseTests<TestClassName> : IDisposable
{
    protected const string _firstFormFileContent = "#first content form file";
    protected const string _secondFormFileContent = "#second content form file";

    private readonly ServiceProvider _fixture;
    protected readonly TestsStartup<TestClassName> Startup;
    protected BaseTests()
    {
        Startup = new TestsStartup<TestClassName>();
    }

    protected IFormFile GetFormFile(string content)
    {
        var memoryStream = new MemoryStream();

        var contentBytes = Encoding.UTF8.GetBytes(content);
        memoryStream.Write(contentBytes, 0, contentBytes.Length);

        memoryStream.Position = 0;
        return new FormFile(
            baseStream: memoryStream,
            baseStreamOffset: 0,
            length: memoryStream.Length,
            name: "formFile",
            fileName: "example.md");
    }
    
    protected IFormFile GetFormImage()
    {
        int width = 200;
        int height = 200;
        
        using var image = new Image<Rgba32>(width, height);
        image.Mutate(ctx =>
        {
            ctx.Resize(new Size(Random.Shared.Next(250),Random.Shared.Next(250)));
        });

        var memoryStream = new MemoryStream();
        
        image.Save(memoryStream, new PngEncoder());
        memoryStream.Position = 0;
        return new FormFile(
            baseStream: memoryStream,
            baseStreamOffset: 0,
            length: memoryStream.Length,
            name: "formFile",
            fileName: "example.png");
    }

    public void Dispose()
    {
        var directory = new DirectoryInfo(Startup.RootPath);

        if (directory.Exists)
        {
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in directory.GetDirectories())
            {
                dir.Delete(true);
            }

            directory.Delete();
        }
    }
    
    
    protected bool CompareByteArrayAndFormFile(byte[] byteArray, IFormFile formFile)
    {
        using var memoryStream = new MemoryStream();
        formFile.CopyTo(memoryStream);
        byte[] fileBytes = memoryStream.ToArray();
        
        if (byteArray.Length == fileBytes.Length)
        {
            for (int i = 0; i < byteArray.Length; i++)
            {
                if (byteArray[i] != fileBytes[i])
                {
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}