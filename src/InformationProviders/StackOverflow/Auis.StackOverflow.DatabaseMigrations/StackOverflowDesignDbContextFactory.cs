namespace Auis.StackOverflow.DatabaseMigrations;

public class StackOverflowDesignDbContextFactory : IDesignTimeDbContextFactory<StackOverflowDbContext>
{
    public StackOverflowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StackOverflowDbContext>();
        optionsBuilder.UseSqlServer("Server=.;Database=Auis_StackOverflow;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true;",
            x =>
            {
                x.MigrationsAssembly("Auis.StackOverflow.DatabaseMigrations");
                x.MigrationsHistoryTable("__MigrationsHistory", StackOverflowDbContext.DbSchemaName);
            });
        optionsBuilder.UseModel(DataAccess.Compiledmodels.StackOverflowDbContextModel.Instance);
        return new StackOverflowDbContext(optionsBuilder.Options);
    }
}