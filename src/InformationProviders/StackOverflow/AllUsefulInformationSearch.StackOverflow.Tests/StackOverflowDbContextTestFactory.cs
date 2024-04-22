namespace AllUsefulInformationSearch.StackOverflow.Tests;

public class StackOverflowDbContextTestFactory : IDesignTimeDbContextFactory<StackOverflowDbContext>
{
    public StackOverflowDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<StackOverflowDbContext>();
        optionsBuilder.UseInMemoryDatabase("StackOverflow");
        return new StackOverflowDbContext(optionsBuilder.Options);
    }
}