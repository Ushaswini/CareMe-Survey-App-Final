namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionOptions : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Questions", "Options", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Questions", "Options");
        }
    }
}
