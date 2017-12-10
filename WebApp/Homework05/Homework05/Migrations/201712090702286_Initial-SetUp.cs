namespace Homework05.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialSetUp : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Devices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        DeviceId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionText = c.String(),
                        QuestionType = c.Int(nullable: false),
                        Options = c.String(),
                        Minimum = c.Double(nullable: false),
                        Maximum = c.Double(nullable: false),
                        StepSize = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.StudyGroups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudyCoordinatorId = c.String(),
                        StudyGroupName = c.String(),
                        StudyGroupCreadtedTime = c.String(),
                        StudyCoordinator_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.StudyCoordinator_Id)
                .Index(t => t.StudyCoordinator_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.SurveyResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        StudyGroupId = c.Int(nullable: false),
                        ResponseText = c.String(),
                        ResponseReceivedTime = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.SurveyId)
                .Index(t => t.UserId)
                .Index(t => t.StudyGroupId);
            
            CreateTable(
                "dbo.Surveys",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyName = c.String(),
                        SurveyType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.X_Survey_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        StudyGroupId = c.Int(nullable: false),
                        SurveyCreatedTime = c.String(),
                        FrequencyOfNotifications = c.Int(nullable: false),
                        Time1 = c.String(),
                        Time2 = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId)
                .Index(t => t.StudyGroupId);
            
            CreateTable(
                "dbo.X_Survey_Question",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SurveyId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .ForeignKey("dbo.Surveys", t => t.SurveyId, cascadeDelete: true)
                .Index(t => t.SurveyId)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.X_User_Group",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        StudyGroupId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .ForeignKey("dbo.StudyGroups", t => t.StudyGroupId, cascadeDelete: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.X_Survey_Question", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.X_Survey_Question", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.X_Survey_Group", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.X_Survey_Group", "StudyGroupId", "dbo.StudyGroups");
            DropForeignKey("dbo.SurveyResponses", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.SurveyResponses", "SurveyId", "dbo.Surveys");
            DropForeignKey("dbo.SurveyResponses", "StudyGroupId", "dbo.StudyGroups");
            DropForeignKey("dbo.StudyGroups", "StudyCoordinator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.X_Survey_Question", new[] { "QuestionId" });
            DropIndex("dbo.X_Survey_Question", new[] { "SurveyId" });
            DropIndex("dbo.X_Survey_Group", new[] { "StudyGroupId" });
            DropIndex("dbo.X_Survey_Group", new[] { "SurveyId" });
            DropIndex("dbo.SurveyResponses", new[] { "StudyGroupId" });
            DropIndex("dbo.SurveyResponses", new[] { "UserId" });
            DropIndex("dbo.SurveyResponses", new[] { "SurveyId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.StudyGroups", new[] { "StudyCoordinator_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.X_User_Group");
            DropTable("dbo.X_Survey_Question");
            DropTable("dbo.X_Survey_Group");
            DropTable("dbo.Surveys");
            DropTable("dbo.SurveyResponses");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.StudyGroups");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Questions");
            DropTable("dbo.Devices");
        }
    }
}
