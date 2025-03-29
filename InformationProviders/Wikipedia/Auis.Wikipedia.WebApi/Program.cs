namespace Auis.Wikipedia.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContextPool<WikipediaDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Auis_Wikipedia"))); //TODO Add precompiled models later
                //.UseModel(DataAccess.Compiledmodels.WikipediaDbContextModel.Instance)); 

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}