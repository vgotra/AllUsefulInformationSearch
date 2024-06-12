using Auis.StackOverflow.Services.Utilities;

namespace Auis.StackOverflow.App;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context)
    {
        var configuration = context.Configuration;
        services.Configure<StackOverflowOptions>(configuration.GetSection(nameof(StackOverflowOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<StackOverflowOptions>>().Value.BaseUrl));
        services.AddMediator(); //TODO Add db context fabric because Mediator registered as Singleton

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

        services.AddTransient<IFileUtilityService>(sp => Environment.OSVersion switch
        {
            { Platform: PlatformID.Win32NT } => new WindowsFileUtilityService(),
            { Platform: PlatformID.Unix } => new LinuxFileUtilityService(),
            _ => throw new InvalidOperationException("Unsupported OS.")
        });

        services.AddTransient<IStackOverflowProcessingWorkflow, StackOverflowProcessingWorkflow>();
    }
}