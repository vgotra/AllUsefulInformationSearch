namespace Auis.StackOverflow.App;

static class Program
{
    static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) => services.ConfigureServices(context))
            .Build();

        var workflow = host.Services.GetRequiredService<IStackOverflowProcessingWorkflow>();
        var cancellationTokenSource = new CancellationTokenSource();
        await workflow.ExecuteAsync(cancellationTokenSource.Token);
    }
}