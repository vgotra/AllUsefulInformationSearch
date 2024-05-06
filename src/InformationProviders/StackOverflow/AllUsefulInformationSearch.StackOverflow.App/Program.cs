namespace AllUsefulInformationSearch.StackOverflow.App;

static class Program
{
    static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        var configuration = builder.Build();
        var serviceCollection = new ServiceCollection();
        
        Startup.ConfigureServices(serviceCollection, configuration);
        
        var sp = serviceCollection.BuildServiceProvider();
        var workflow = sp.GetRequiredService<IStackOverflowProcessingWorkflow>();
        var cancellationTokenSource = new CancellationTokenSource();
        await workflow.ExecuteAsync(cancellationTokenSource.Token);
    }
}