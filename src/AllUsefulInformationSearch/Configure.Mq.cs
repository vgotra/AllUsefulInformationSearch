using ServiceStack.Messaging;

[assembly: HostingStartup(typeof(ConfigureMq))]

namespace AllUsefulInformationSearch;

/**
  Register Services you want available via MQ in your AppHost, e.g:
    var mqServer = container.Resolve<IMessageService>();
    mqServer.RegisterHandler<MyRequest>(ExecuteMessage);
*/
public class ConfigureMq : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices(services => {
            services.AddSingleton<IMessageService>(c => new BackgroundMqService());
        })
        .ConfigureAppHost(afterAppHostInit: appHost => {
            appHost.Resolve<IMessageService>().Start();
        });
}