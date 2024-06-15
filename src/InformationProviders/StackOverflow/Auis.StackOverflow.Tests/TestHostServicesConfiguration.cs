namespace Auis.StackOverflow.Tests;

public static class TestHostServicesConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context, bool inMemoryDb = true)
    {
        var configuration = context.Configuration;
        services.Configure<StackOverflowOptions>(configuration.GetSection(nameof(StackOverflowOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<StackOverflowOptions>>().Value.BaseUrl));
        services.AddMediator();

        services.AddSingleton(inMemoryDb
            ? new DbContextOptionsBuilder<StackOverflowDbContext>().UseInMemoryDatabase("Auis_StackOverflow").Options
            : new DbContextOptionsBuilder<StackOverflowDbContext>().UseSqlServer(configuration.GetConnectionString("Auis_StackOverflow")).Options);
        services.AddSingleton<IDbContextFactory<StackOverflowDbContext>, StackOverflowDbContextFactory>();

        services.AddTransient<IWebDataFilesRepository, WebDataFilesRepositoryMock>();
        services.AddTransient<IFileUtilityService>(_ => Environment.OSVersion switch
        {
            { Platform: PlatformID.Win32NT } => new WindowsFileUtilityService(),
            { Platform: PlatformID.Unix } => new LinuxFileUtilityService(),
            _ => throw new InvalidOperationException("Unsupported OS.")
        });

        services.AddTransient<IParsingService, ParsingService>();
        services.AddTransient<IStackOverflowProcessingWorkflow, StackOverflowProcessingWorkflow>();
    }
}