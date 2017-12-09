namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeviceIdTables : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DeviceIdModels",
                c => new
                    {
                        DeviceId = c.String(nullable: false, maxLength: 400),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.DeviceId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DeviceIdModels");
        }
    }
}
