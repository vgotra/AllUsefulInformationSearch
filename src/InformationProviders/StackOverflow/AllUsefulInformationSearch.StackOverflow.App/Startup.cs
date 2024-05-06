namespace AllUsefulInformationSearch.StackOverflow.App;

public static class Startup
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddLogging(configure => configure.AddConfiguration(configuration.GetSection("Logging")).AddConsole());

        var stackOverflowBaseUri = new Uri(configuration["StackOverflow:BaseUrl"] ?? throw new InvalidOperationException("StackOverflow base URL is not specified."));

        services.AddHttpClient("", (provider, client) => client.BaseAddress = stackOverflowBaseUri);

        //TODO Find a way to register DbContextPool as Transient, also batches
        services.AddDbContext<StackOverflowDbContext>(
            options =>
            {
                options.UseModel(DataAccess.Compiledmodels.StackOverflowDbContextModel.Instance);
                options.UseSqlServer(configuration.GetConnectionString("AllUsefulInformationSearch_StackOverflow"));
            },
            ServiceLifetime.Transient,
            ServiceLifetime.Singleton);

        services.AddTransient<IWebDataFilesRepository, WebDataFilesRepository>();

        services.AddTransient<IFileUtilityService>(_ => Environment.OSVersion switch
        {
            { Platform: PlatformID.Win32NT } => new WindowsFileUtilityService(),
            { Platform: PlatformID.Unix } => new LinuxFileUtilityService(),
            _ => throw new InvalidOperationException("Unsupported OS.")
        });
        services.AddTransient<IWebDataFilesService, WebDataFilesService>();
        services.AddTransient<IWebArchiveFileService, WebArchiveFileService>();
        services.AddTransient<IPostModificationService, PostModificationService>();
        services.AddTransient<IPostsSynchronizationService, PostsSynchronizationService>();
        services.AddTransient<IWebArchiveParserService, WebArchiveParserService>();

        services.AddTransient<IStackOverflowProcessingSubWorkflow, StackOverflowProcessingSubWorkflow>();
        services.AddTransient<IStackOverflowProcessingWorkflow, StackOverflowProcessingWorkflow>();
    }
}