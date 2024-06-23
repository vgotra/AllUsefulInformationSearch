namespace Auis.StackOverflow.App;

static class Program
{
    static async Task Main(string[]? args)
    {
        // if we have correct arguments specified, then process the file, otherwise process all files

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => services.ConfigureServices(context))
            .Build();

        if (args == null || args.Length == 0)
        {
            Console.WriteLine("Started processing all StackOverflow files");

            var workflow = host.Services.GetRequiredService<IStackOverflowProcessingWorkflow>();
            await workflow.ExecuteAsync();

            Console.WriteLine("Completed processing all StackOverflow files");
            return;
        }

        var dbContextFactory = host.Services.GetRequiredService<IDbContextFactory<StackOverflowDbContext>>();
        await using var dbContext = await dbContextFactory.CreateDbContextAsync();

        var fileName = args[0];
        var webDataFile = await dbContext.WebDataFiles.AsTracking().FirstOrDefaultAsync(x => x.Name == fileName);
        if (webDataFile == null)
        {
            await Console.Error.WriteLineAsync($"StackOverflow file: '{fileName}' not found.");
            return;
        }

        Console.WriteLine($"Started processing StackOverflow file: '{fileName}'");

        var subWorkflow = host.Services.GetRequiredService<IStackOverflowProcessingSubWorkflow>();
        await subWorkflow.ExecuteAsync(webDataFile);

        Console.WriteLine($"Completed processing StackOverflow file: '{fileName}'");
    }
}