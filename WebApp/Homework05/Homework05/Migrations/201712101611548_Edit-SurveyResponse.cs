namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EditSurveyResponse : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.SurveyResponses", "StudyGroupId", "dbo.StudyGroups");
            DropIndex("dbo.SurveyResponses", new[] { "StudyGroupId" });
            AddColumn("dbo.SurveyResponses", "QuestionId", c => c.Int(nullable: false));
            AlterColumn("dbo.X_User_Group", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.SurveyResponses", "QuestionId");
            CreateIndex("dbo.X_User_Group", "UserId");
            CreateIndex("dbo.X_User_Group", "StudyGroupId");
            AddForeignKey("dbo.SurveyResponses", "QuestionId", "dbo.Questions", "Id", cascadeDelete: true);
            
           
            DropColumn("dbo.SurveyResponses", "StudyGroupId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.SurveyResponses", "StudyGroupId", c => c.Int(nullable: false));
            
            
            DropForeignKey("dbo.SurveyResponses", "QuestionId", "dbo.Questions");
            DropIndex("dbo.X_User_Group", new[] { "StudyGroupId" });
            DropIndex("dbo.X_User_Group", new[] { "UserId" });
            DropIndex("dbo.SurveyResponses", new[] { "QuestionId" });
            AlterColumn("dbo.X_User_Group", "UserId", c => c.String());
            DropColumn("dbo.SurveyResponses", "QuestionId");
            CreateIndex("dbo.SurveyResponses", "StudyGroupId");
            AddForeignKey("dbo.SurveyResponses", "StudyGroupId", "dbo.StudyGroups", "Id", cascadeDelete: true);
        }
    }
}
