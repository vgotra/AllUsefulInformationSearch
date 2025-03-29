namespace Auis.Wikipedia.DatabaseMigrations;

public class WikipediaDesignDbContextFactory : IDesignTimeDbContextFactory<WikipediaDbContext>
{
    public WikipediaDbContext CreateDbContext(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).Build();

        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var logger = host.Services.GetRequiredService<ILogger<WikipediaDesignDbContextFactory>>();
        var connectionString = configuration.GetConnectionString("Auis_Wikipedia");

        logger.LogInformation("Current Environment: {Environment}", host.Services.GetRequiredService<IHostEnvironment>().EnvironmentName);
        logger.LogInformation("Connection String: {ConnectionString}", connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<WikipediaDbContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Auis.Wikipedia.DatabaseMigrations")
            .MigrationsHistoryTable("__MigrationsHistory", WikipediaDbContext.DbSchemaName));

        return new WikipediaDbContext(optionsBuilder.Options);
    }
}