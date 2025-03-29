namespace Auis.Wikipedia.App;

static class Program
{
    static async Task Main(string[]? args)
    {
        // if we have correct arguments specified, then process the file, otherwise process all files

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => services.ConfigureServices(context, args?.Length > 0))
            .Build();

        if (args == null || args.Length == 0)
        {
            Console.WriteLine("Started processing all Wikipedia files");

            var workflow = host.Services.GetRequiredService<IWikipediaProcessingWorkflow>();
            await workflow.ExecuteAsync();

            Console.WriteLine("Completed processing all Wikipedia files");
            return;
        }

        var dbContextFactory = host.Services.GetRequiredService<IDbContextFactory<WikipediaDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var fileName = args[0];
        var webDataFile = await dbContext.WebDataFiles.AsTracking().FirstOrDefaultAsync(x => x.Name == fileName);
        if (webDataFile == null)
        {
            await Console.Error.WriteLineAsync($"Wikipedia file: '{fileName}' not found.");
            return;
        }

        Console.WriteLine($"Started processing Wikipedia file: '{fileName}'");



        var subWorkflow = host.Services.GetRequiredService<IWikipediaProcessingSubWorkflow>();
        await subWorkflow.ExecuteAsync(webDataFile);

        Console.WriteLine($"Completed processing Wikipedia file: '{fileName}'");
    }
}