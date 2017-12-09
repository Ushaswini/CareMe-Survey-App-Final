namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdToDeviceTable : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Devices");
            AddColumn("dbo.Devices", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Devices", "DeviceId", c => c.String());
            AddPrimaryKey("dbo.Devices", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Devices");
            AlterColumn("dbo.Devices", "DeviceId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Devices", "Id");
            AddPrimaryKey("dbo.Devices", "DeviceId");
        }
    }
}
