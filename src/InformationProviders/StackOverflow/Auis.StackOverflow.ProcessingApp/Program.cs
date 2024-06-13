namespace Auis.StackOverflow.ProcessingApp;

static class Program
{
    static async Task Main(string[]? args)
    {
        if (args == null || args.Length == 0)
        {
            await Console.Error.WriteLineAsync("Please provide name of file to process.");
            return;
        }

        var host = Host.CreateDefaultBuilder(args) // it will use default appsettings.json file from common folder
            .ConfigureServices((context, services) => services.ConfigureSubServices(context))
            .Build();

        var fileName = args[0];
        var webDataFile = await host.Services.GetRequiredService<IDbContextFactory<StackOverflowDbContext>>().CreateDbContext()
            .WebDataFiles.AsTracking().FirstOrDefaultAsync(x => x.Name == fileName);
        if (webDataFile == null)
        {
            await Console.Error.WriteLineAsync($"File {fileName} not found.");
            return;
        }

        Console.WriteLine($"Started processing file {fileName}");
        await host.Services.GetRequiredService<IMediator>().Send(new PostsProcessingCommand(webDataFile));
        Console.WriteLine($"Completed processing file {fileName}");
    }
}