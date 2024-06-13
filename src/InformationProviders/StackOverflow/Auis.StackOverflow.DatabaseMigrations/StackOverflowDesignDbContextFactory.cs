namespace Auis.StackOverflow.DatabaseMigrations;

public class StackOverflowDesignDbContextFactory : IDesignTimeDbContextFactory<StackOverflowDbContext>
{
    public StackOverflowDbContext CreateDbContext(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).Build();

        var configuration = host.Services.GetRequiredService<IConfiguration>();
        var logger = host.Services.GetRequiredService<ILogger<StackOverflowDesignDbContextFactory>>();
        var connectionString = configuration.GetConnectionString("Auis_StackOverflow");

        logger.LogInformation("Current Environment: {Environment}", host.Services.GetRequiredService<IHostEnvironment>().EnvironmentName);
        logger.LogInformation("Connection String: {ConnectionString}", connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<StackOverflowDbContext>();
        optionsBuilder.UseSqlServer(connectionString, x => x.MigrationsAssembly("Auis.StackOverflow.DatabaseMigrations")
            .MigrationsHistoryTable("__MigrationsHistory", StackOverflowDbContext.DbSchemaName));

        return new StackOverflowDbContext(optionsBuilder.Options);
    }
}