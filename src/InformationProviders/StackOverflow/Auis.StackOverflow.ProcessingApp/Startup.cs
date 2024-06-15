namespace Auis.StackOverflow.ProcessingApp;

public static class Startup
{
    public static void ConfigureSubServices(this IServiceCollection services, HostBuilderContext context)
    {
        var configuration = context.Configuration;
        services.Configure<StackOverflowOptions>(configuration.GetSection(nameof(StackOverflowOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<StackOverflowOptions>>().Value.BaseUrl));
        services.AddMediator();
        services.AddSingleton(new DbContextOptionsBuilder<StackOverflowDbContext>().UseSqlServer(configuration.GetConnectionString("Auis_StackOverflow"))
            .UseModel(DataAccess.Compiledmodels.StackOverflowDbContextModel.Instance).Options);
        services.AddSingleton<IDbContextFactory<StackOverflowDbContext>, StackOverflowDbContextFactory>();

        services.AddTransient<IWebDataFilesRepository, WebDataFilesRepository>();
        services.AddTransient<IFileUtilityService>(sp => Environment.OSVersion switch
        {
            { Platform: PlatformID.Win32NT } => new WindowsFileUtilityService(),
            { Platform: PlatformID.Unix } => new LinuxFileUtilityService(),
            _ => throw new InvalidOperationException("Unsupported OS.")
        });

        services.AddTransient<IParsingService, ParsingService>();
    }
}