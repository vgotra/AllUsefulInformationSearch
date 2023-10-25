using AllUsefulInformationSearch.DatabaseMigrations.Migrations;
using Microsoft.Extensions.Configuration;
using ServiceStack.OrmLite;

//TODO Add console command to run migrations
var config = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .AddJsonFile("appsettings.Development.json")
            .Build();

var connectionString = config.GetConnectionString("DefaultConnection");
var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD") ?? throw new InvalidOperationException("Admin password was not found in Environment variables");
connectionString = $"{connectionString};Password={adminPassword};";
var dbConnectionFactory = new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider);
var migrator = new Migrator(dbConnectionFactory, typeof(Migration1000).Assembly);

//TODO Add try/catch
migrator.Run();