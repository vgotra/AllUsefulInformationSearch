namespace AllUsefulInformationSearch.StackOverflow.App;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(configure => configure.AddConsole());
        
        var stackOverflowBaseUri = new Uri(configuration["StackOverflow:BaseUrl"] ?? throw new InvalidOperationException("StackOverflow base URL is not specified."));

        services.AddHttpClient("", (provider, client) => client.BaseAddress = stackOverflowBaseUri);
        // services.AddHttpClient("StackOverflow", client => client.BaseAddress = stackOverflowBaseUri);

        services.AddDbContextFactory<StackOverflowDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AllUsefulInformationSearch_StackOverflow")));

        services.AddTransient<IWebDataFilesRepository, WebDataFilesRepository>();

        services.AddTransient<IFileUtilityService, WindowsFileUtilityService>(); // Add later for Linux, etc.
        services.AddTransient<IWebDataFilesService, WebDataFilesService>();
        services.AddTransient<IWebArchiveFileService, WebArchiveFileService>();
        services.AddTransient<IPostModificationService, PostModificationService>();
        services.AddTransient<IPostsSynchronizationService, PostsSynchronizationService>();
        services.AddTransient<IWebArchiveParserService, WebArchiveParserService>();

        services.AddTransient<IWorkflow, StackOverflowProcessingWorkflow>();
    }
}