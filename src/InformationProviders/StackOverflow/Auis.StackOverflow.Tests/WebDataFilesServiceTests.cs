﻿namespace Auis.StackOverflow.Tests;

[TestClass]
public class WebDataFilesServiceTests : BaseTests
{
    [TestMethod]
    public async Task CanDownloadAndParseAndSaveFilesToDb()
    {
        var httpClient = new HttpClient { BaseAddress = new Uri("https://archive.org/download/stackexchange/") };
        var cancellationTokenSource = new CancellationTokenSource();
        var dbContext = new StackOverflowDbContextTestFactory().CreateDbContext(null!);
        var parser = new WebArchiveParserService(httpClient, new TestContextLogger<WebArchiveParserService>(TestContext));
        var service = new WebDataFilesService(dbContext, parser, new TestContextLogger<WebDataFilesService>(TestContext));
        await service.SynchronizeWebDataFilesAsync(cancellationTokenSource.Token);
        var itemsCount = await dbContext.WebDataFiles.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(itemsCount > 0);

        var webDataFile = await dbContext.WebDataFiles.OrderBy(x => x.Size).FirstOrDefaultAsync(cancellationToken: cancellationTokenSource.Token);
        Assert.IsNotNull(webDataFile);

        IFileUtilityService fileUtilityService = Environment.OSVersion.Platform == PlatformID.Win32NT ? new WindowsFileUtilityService(httpClient) : new LinuxFileUtilityService(httpClient); // add MacOS later
        
        var paths = webDataFile.ToWebFilePaths();
        var posts = await new WebArchiveFileService(fileUtilityService, new TestContextLogger<WebArchiveFileService>(TestContext)).GetPostsWithCommentsAsync(paths, cancellationTokenSource.Token);
        Assert.IsTrue(posts.Count > 0);

        var entities = posts.Select(x => x.ToEntity());
        await dbContext.Posts.AddRangeAsync(entities, cancellationTokenSource.Token);
        await dbContext.SaveChangesAsync(cancellationTokenSource.Token);

        var postCount = await dbContext.Posts.CountAsync(cancellationTokenSource.Token);
        Assert.IsTrue(posts.Count == postCount);
    }
}