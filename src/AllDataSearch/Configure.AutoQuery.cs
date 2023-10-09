[assembly: HostingStartup(typeof(ConfigureAutoQuery))]

namespace AllDataSearch;

public class ConfigureAutoQuery : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureAppHost(appHost => {
            appHost.Plugins.Add(new AutoQueryFeature {
                MaxLimit = 1000,
                IncludeTotal = true,
                GenerateCrudServices = new GenerateCrudServices {
                    AutoRegister = true,
                    //AddDataContractAttributes = false,
                }
            });
        });
}