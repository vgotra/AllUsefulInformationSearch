using ServiceStack.Api.OpenApi;

[assembly: HostingStartup(typeof(ConfigureOpenApi))]

namespace AllDataSearch;

public class ConfigureOpenApi : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureAppHost(appHost => {
            appHost.Plugins.Add(new OpenApiFeature());
        });
}