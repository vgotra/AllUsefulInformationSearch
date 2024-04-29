using AllUsefulInformationSearch.StackOverflow.Workflows.Extensions;

namespace AllUsefulInformationSearch.StackOverflow.Tests;

[TestClass]
public class WebDataFilesServiceTests : BaseTests
{
    [TestMethod]
    public async Task CanDownloadAndParseAndSaveFilesToDb()
    {
        const string stackOverflowArchiveUrl = "https://archive.org/download/stackexchange";

        var dbContext = new StackOverflowDbContextTestFactory().CreateDbContext(null!);
        var repository = new WebDataFilesRepository(dbContext);
        var logger = new TestContextLogger<WebArchiveParser>(TestContext);
        var parser = new WebArchiveParser(logger);
        var service = new WebDataFilesService(repository, parser);
        await service.SynchronizeWebDataFilesAsync();
        var itemsCount = await dbContext.WebDataFiles.CountAsync();
        Assert.IsTrue(itemsCount > 0);

        var webDataFile = await dbContext.WebDataFiles.FirstOrDefaultAsync(x => x.Name.StartsWith("3dprinting.meta.stackexchange.com"));
        Assert.IsNotNull(webDataFile);

        IFileUtilityService fileUtilityService = Environment.OSVersion.Platform == PlatformID.Win32Windows ? new WindowsFileUtilityService() : new LinuxFileUtilityService(); // add MacOS later
        var fileUri = $"{stackOverflowArchiveUrl}/{webDataFile.Link}";
        var paths = new WebFilePaths { WebFileUri = fileUri, TemporaryDownloadPath = Path.GetTempFileName(), ArchiveOutputDirectory = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()) };
        var posts = await new WebArchiveFileService(fileUtilityService).GetPostsWithCommentsAsync(paths);
        Assert.IsTrue(posts.Count > 0);
        
        var entities = posts.Select(x => x.ToEntity()).ToList();
        await dbContext.Posts.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
        
        var postCount = await dbContext.Posts.CountAsync();
        Assert.IsTrue(posts.Count == postCount);
    }
}