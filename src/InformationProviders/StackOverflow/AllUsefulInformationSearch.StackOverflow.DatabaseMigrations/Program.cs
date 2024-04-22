namespace AllUsefulInformationSearch.StackOverflow.DatabaseMigrations;

static class Program
{
    static async Task Main(string[] args)
    {
        await Task.CompletedTask;
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration configuration = builder.Build();

        var options = configuration.GetSection("DatabaseMigrationOptions").Get<DatabaseMigrationOptions>()
                      ?? throw new InvalidOperationException("DatabaseMigrationOptions section is missing in appsettings.json");
        
        var loggerFactory = LoggerFactory.Create(b => b.AddConsole());
        //var logger = loggerFactory.CreateLogger<DatabaseMigrationService>();
        //var dbConnectionFactory = new DbConnectionFactory(options.ConnectionString);

        //var service = new DatabaseMigrationService(options, dbConnectionFactory, logger);
        //await service.ApplyMigrationsAsync();
    }
}