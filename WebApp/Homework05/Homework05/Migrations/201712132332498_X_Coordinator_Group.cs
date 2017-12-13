namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class X_Coordinator_Group : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.X_Coordinator_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CoordinatorId = c.String(maxLength: 128),
                        StudyGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.CoordinatorId, cascadeDelete:true)
                .Index(t => t.StudyGroupId)
                .Index(t => t.CoordinatorId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.X_Coordinator_Group", "CoordinatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.X_Coordinator_Group", "StudyGroupId", "dbo.StudyGroups");
            DropIndex("dbo.X_Coordinator_Group", new[] { "CoordinatorId" });
            DropIndex("dbo.X_Coordinator_Group", new[] { "StudyGroupId" });
            DropTable("dbo.X_Coordinator_Group");
        }
    }
}
