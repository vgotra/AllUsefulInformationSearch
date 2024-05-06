using Microsoft.EntityFrameworkCore.Design;

namespace AllUsefulInformationSearch.StackOverflow.DataAccess;

public class StackOverflowDesignDbContextFactory : IDesignTimeDbContextFactory<StackOverflowDbContext>
{
    public StackOverflowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StackOverflowDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=AllUsefulInformationSearch_StackOverflow;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;",
            x =>
            {
                x.MigrationsAssembly("AllUsefulInformationSearch.StackOverflow.DatabaseMigrations");
                x.MigrationsHistoryTable("__MigrationsHistory", StackOverflowDbContext.DbSchemaName);
            });
        return new StackOverflowDbContext(optionsBuilder.Options);
    }
}