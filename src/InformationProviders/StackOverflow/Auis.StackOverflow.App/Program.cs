namespace Auis.StackOverflow.App;

static class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => services.ConfigureServices(context))
            .Build();

        await host.Services.GetRequiredService<IStackOverflowProcessingWorkflow>().ExecuteAsync();
    }
}