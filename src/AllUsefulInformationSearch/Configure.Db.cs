using ServiceStack.Data;
using ServiceStack.OrmLite;

[assembly: HostingStartup(typeof(ConfigureDb))]

namespace AllUsefulInformationSearch;

public class ConfigureDb : IHostingStartup
{
    public void Configure(IWebHostBuilder builder) => builder
        .ConfigureServices((context, services) => {
            services.AddSingleton<IDbConnectionFactory>(new OrmLiteConnectionFactory(
                context.Configuration.GetConnectionString("DefaultConnection")
                ?? "Server=localhost;User Id=test;Password=test;Database=test;Pooling=true;MinPoolSize=0;MaxPoolSize=200",
                PostgreSqlDialect.Provider));
        })
        .ConfigureAppHost(appHost => {
            // Enable built-in Database Admin UI at /admin-ui/database
            appHost.Plugins.Add(new AdminDatabaseFeature());
        });
}
