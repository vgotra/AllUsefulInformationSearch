using Auis.StackOverflow.Common.Options;

namespace Auis.StackOverflow.Tests;

public static class TestHostServicesConfiguration
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context, bool inMemoryDb = true)
    {
        var configuration = context.Configuration;
        services.Configure<StackOverflowOptions>(configuration.GetSection(nameof(StackOverflowOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<StackOverflowOptions>>().Value.BaseUrl));

        services.AddSingleton(inMemoryDb
            ? new DbContextOptionsBuilder<StackOverflowDbContext>().UseInMemoryDatabase("Auis_StackOverflow").Options
            : new DbContextOptionsBuilder<StackOverflowDbContext>().UseNpgsql(configuration.GetConnectionString("Auis_StackOverflow")).Options);
        services.AddSingleton<IDbContextFactory<StackOverflowDbContext>, StackOverflowDbContextFactory>();

        services.AddStackOverflowServices();
        services.AddTransient<PostsArchiveFileProcessingServiceMock>();
    }
}