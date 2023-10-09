using ServiceStack;

[assembly: HostingStartup(typeof(AllDataSearch.ConfigureCors))]

namespace AllDataSearch
{
    public class ConfigureCors : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureAppHost(appHost => {
                appHost.Plugins.Add(new CorsFeature());
            });
    }
}