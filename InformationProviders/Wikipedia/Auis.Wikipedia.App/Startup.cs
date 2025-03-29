using Auis.Wikipedia.Common.Options;

namespace Auis.Wikipedia.App;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context, bool isSubProcess = false)
    {
        var configuration = context.Configuration;
        services.Configure<WikipediaOptions>(configuration.GetSection(nameof(WikipediaOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<WikipediaOptions>>().Value.BaseUrl));
        services.AddSingleton(new DbContextOptionsBuilder<WikipediaDbContext>().UseSqlServer(configuration.GetConnectionString("Auis_Wikipedia")));
            // .UseModel(DataAccess.Compiledmodels.WikipediaDbContextModel.Instance).Options); //TODO Add precompiled models later
        services.AddSingleton<IDbContextFactory<WikipediaDbContext>, WikipediaDbContextFactory>();

        if (isSubProcess)
            services.Configure<WikipediaOptions>(options => options.UseSubProcessForProcessingFile = false);

        services.AddWikipediaServices();
    }
}