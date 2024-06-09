namespace Auis.StackOverflow.Tests;

public static class TestHostServicesConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context)
    {
        var configuration = context.Configuration;
        services.Configure<StackOverflowOptions>(configuration.GetSection(nameof(StackOverflowOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<StackOverflowOptions>>().Value.BaseUrl));

        //TODO Find a way to register DbContextPool as Transient, also batches
        services.AddDbContext<StackOverflowDbContext>(
            options =>
            {
                options.UseModel(DataAccess.Compiledmodels.StackOverflowDbContextModel.Instance);
                options.UseSqlServer(configuration.GetConnectionString("Auis_StackOverflow"));
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
        services.AddTransient<IArchiveFileService, ArchiveFileService>();
        services.AddTransient<IPostModificationService, PostModificationService>();
        services.AddTransient<IPostsSynchronizationService, PostsSynchronizationService>();
        services.AddTransient<IWebArchiveParserService, WebArchiveParserService>();

        services.AddTransient<IStackOverflowProcessingSubWorkflow, StackOverflowProcessingSubWorkflow>();
        services.AddTransient<IStackOverflowProcessingWorkflow, StackOverflowProcessingWorkflow>();
    }
}