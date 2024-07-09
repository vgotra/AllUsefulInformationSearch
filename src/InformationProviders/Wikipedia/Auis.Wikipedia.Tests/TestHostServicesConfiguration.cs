using Auis.Wikipedia.Common.Options;

namespace Auis.Wikipedia.Tests;

public static class TestHostServicesConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context, bool inMemoryDb = true)
    {
        var configuration = context.Configuration;
        services.Configure<WikipediaOptions>(configuration.GetSection(nameof(WikipediaOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<WikipediaOptions>>().Value.BaseUrl));

        services.AddSingleton(inMemoryDb
            ? new DbContextOptionsBuilder<WikipediaDbContext>().UseInMemoryDatabase("Auis_Wikipedia").Options
            : new DbContextOptionsBuilder<WikipediaDbContext>().UseSqlServer(configuration.GetConnectionString("Auis_Wikipedia")).Options);
        services.AddSingleton<IDbContextFactory<WikipediaDbContext>, WikipediaDbContextFactory>();

        services.AddWikipediaServices();
        services.AddTransient<PostsArchiveFileProcessingServiceMock>();
    }
}