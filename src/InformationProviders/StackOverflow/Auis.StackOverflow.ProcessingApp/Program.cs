namespace Auis.StackOverflow.ProcessingApp;

static class Program
{
    static async Task Main(string[]? args)
    {
        if (args == null || args.Length == 0)
        {
            Console.WriteLine("Please provide name of file to process.");
            return;
        }

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => services.ConfigureServices(context))
            .Build();

        await host.Services.GetRequiredService<IStackOverflowProcessingWorkflow>().ExecuteAsync();
    }
}