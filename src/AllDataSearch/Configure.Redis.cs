using ServiceStack.Redis;

[assembly: HostingStartup(typeof(ConfigureRedis))]

namespace AllDataSearch;

public class ConfigureRedis : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) => {
            services.AddSingleton<IRedisClientsManager>(
                new RedisManagerPool(context.Configuration.GetConnectionString("Redis") ?? "localhost:6379"));
        })
        .ConfigureAppHost(appHost => {
            // Enable built-in Redis Admin UI at /admin-ui/redis
            // appHost.Plugins.Add(new AdminRedisFeature());
        });
}