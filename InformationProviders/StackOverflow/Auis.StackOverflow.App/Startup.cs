using Auis.StackOverflow.Common.Options;

namespace Auis.StackOverflow.App;

public static class Startup
{
    public static void ConfigureServices(this IServiceCollection services, HostBuilderContext context, bool isSubProcess = false)
    {
        var configuration = context.Configuration;
        services.Configure<StackOverflowOptions>(configuration.GetSection(nameof(StackOverflowOptions)));
        services.AddHttpClient("", (sp, client) => client.BaseAddress = new Uri(sp.GetRequiredService<IOptions<StackOverflowOptions>>().Value.BaseUrl));
        services.AddSingleton(new DbContextOptionsBuilder<StackOverflowDbContext>().UseSqlServer(configuration.GetConnectionString("Auis_StackOverflow"))
            .UseModel(DataAccess.Compiledmodels.StackOverflowDbContextModel.Instance).Options);
        services.AddSingleton<IDbContextFactory<StackOverflowDbContext>, StackOverflowDbContextFactory>();

        if (isSubProcess)
            services.Configure<StackOverflowOptions>(options => options.UseSubProcessForProcessingFile = false);

        services.AddStackOverflowServices();
    }
}