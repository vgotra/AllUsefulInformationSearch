using AllDataSearch.DataAccess.Entities;
using ServiceStack.OrmLite;

namespace AllDataSearch.DatabaseMigrations.Migrations;

public class Migration1000 : MigrationBase
{
    public override void Up()
    {
        Db.CreateTable<InformationDataFile>();
    }

    public override void Down()
    {
        Db.DropTable<InformationDataFile>();
    }
}
