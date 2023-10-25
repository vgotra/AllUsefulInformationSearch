namespace AllUsefulInformationSearch.DatabaseMigrations.Migrations;

[Description("Create initial database tables")]
public class Migration1000 : MigrationBase
{
    public override void Up()
    {
        Db!.CreateCommand().ExecNonQuery("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";"); // Enable Guid auto generation
        Db.CreateTable<InformationSourceDataFile>();
    }

    public override void Down() => Db.DropTable<InformationSourceDataFile>();
}
