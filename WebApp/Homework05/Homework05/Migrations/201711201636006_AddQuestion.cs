namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddQuestion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Surveys", "QuestionId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Surveys", "QuestionId");
            AddForeignKey("dbo.Surveys", "QuestionId", "dbo.Questions", "QuestionId");
            DropColumn("dbo.Surveys", "QuestionText");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Surveys", "QuestionText", c => c.String());
            DropForeignKey("dbo.Surveys", "QuestionId", "dbo.Questions");
            DropIndex("dbo.Surveys", new[] { "QuestionId" });
            DropColumn("dbo.Surveys", "QuestionId");
        }
    }
}
