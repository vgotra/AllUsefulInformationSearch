using ServiceStack;

[assembly: HostingStartup(typeof(AllDataSearch.ConfigureServerEvents))]

namespace AllDataSearch
{
    public class ConfigureServerEvents : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder) => builder
            .ConfigureAppHost(appHost => {
                appHost.Plugins.Add(new ServerEventsFeature());
            });
    }
}