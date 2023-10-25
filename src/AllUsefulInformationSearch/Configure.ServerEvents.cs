[assembly: HostingStartup(typeof(ConfigureServerEvents))]

namespace AllUsefulInformationSearch;

public class ConfigureServerEvents : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureAppHost(appHost => {
            appHost.Plugins.Add(new ServerEventsFeature());
        });
}