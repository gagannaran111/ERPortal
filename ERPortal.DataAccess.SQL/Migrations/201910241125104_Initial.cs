namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Text = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LinkedApplication_Id = c.String(nullable: false, maxLength: 128),
                        LinkedUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.LinkedApplication_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserAccounts", t => t.LinkedUser_Id, cascadeDelete: true)
                .Index(t => t.LinkedApplication_Id)
                .Index(t => t.LinkedUser_Id);
            
            CreateTable(
                "dbo.ERApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FieldName = c.String(),
                        DateOfDiscovery = c.DateTime(nullable: false),
                        DateOfInitialCommercialProduction = c.DateTime(),
                        DateOfLastCommercialProduction = c.DateTime(),
                        PresentlyUnderProduction = c.Boolean(),
                        ERScreeningStatus = c.Boolean(),
                        FieldOIIP = c.Int(),
                        FieldGIIP = c.Int(),
                        PilotDesign = c.Boolean(),
                        PilotProductionProfile = c.Int(),
                        TechnicallyCompatible = c.Boolean(),
                        EconomicViability = c.Boolean(),
                        AdditonalRemarks = c.String(),
                        SubmissionDate = c.DateTime(),
                        EligibleForFiscalIncentive = c.Boolean(),
                        DGHApprovalStatus = c.Boolean(),
                        DGHApprovalDate = c.DateTime(),
                        DGHFileAttachment = c.String(),
                        PilotMandatory = c.Boolean(),
                        PilotReportApprovalStatus = c.Boolean(),
                        DGHFileAttachmentForPilot = c.String(),
                        FinalApprovalStatus = c.Boolean(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        FieldTypes_Id = c.String(nullable: false, maxLength: 128),
                        OperatorName_Id = c.String(nullable: false, maxLength: 128),
                        ScreeningReport_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FieldTypes", t => t.FieldTypes_Id)
                .ForeignKey("dbo.Operators", t => t.OperatorName_Id)
                .ForeignKey("dbo.ERScreeningDetails", t => t.ScreeningReport_Id)
                .Index(t => t.FieldTypes_Id)
                .Index(t => t.OperatorName_Id)
                .Index(t => t.ScreeningReport_Id);
            
            CreateTable(
                "dbo.FieldTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Operators",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ERScreeningDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ReportDocumentPath = c.String(),
                        FirstOrderScreening = c.Boolean(),
                        SecondOrderScreening = c.Boolean(),
                        ThirdOrderScreening = c.Boolean(),
                        ApprovalStatus = c.Boolean(),
                        DateOfSubmission = c.DateTime(nullable: false),
                        DateOfLastApproval = c.DateTime(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        IssuingInstitute_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERScreeningInstitutes", t => t.IssuingInstitute_Id)
                .Index(t => t.IssuingInstitute_Id);
            
            CreateTable(
                "dbo.ERScreeningInstitutes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        InstituteName = c.String(),
                        ContactPerson = c.String(),
                        Address = c.String(),
                        EmailID = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserAccounts",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        EmailID = c.String(nullable: false),
                        FirstName = c.String(),
                        LastName = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        OperatorName_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Operators", t => t.OperatorName_Id, cascadeDelete: true)
                .Index(t => t.OperatorName_Id);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Text = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LinkedERApplication_Id = c.String(maxLength: 128),
                        UserID_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.LinkedERApplication_Id, cascadeDelete: true)
                .ForeignKey("dbo.UserAccounts", t => t.UserID_Id, cascadeDelete: true)
                .Index(t => t.LinkedERApplication_Id)
                .Index(t => t.UserID_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserID_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.Notifications", "LinkedERApplication_Id", "dbo.ERApplications");
            DropForeignKey("dbo.Comments", "LinkedUser_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "OperatorName_Id", "dbo.Operators");
            DropForeignKey("dbo.Comments", "LinkedApplication_Id", "dbo.ERApplications");
            DropForeignKey("dbo.ERApplications", "ScreeningReport_Id", "dbo.ERScreeningDetails");
            DropForeignKey("dbo.ERScreeningDetails", "IssuingInstitute_Id", "dbo.ERScreeningInstitutes");
            DropForeignKey("dbo.ERApplications", "OperatorName_Id", "dbo.Operators");
            DropForeignKey("dbo.ERApplications", "FieldTypes_Id", "dbo.FieldTypes");
            DropIndex("dbo.Notifications", new[] { "UserID_Id" });
            DropIndex("dbo.Notifications", new[] { "LinkedERApplication_Id" });
            DropIndex("dbo.UserAccounts", new[] { "OperatorName_Id" });
            DropIndex("dbo.ERScreeningDetails", new[] { "IssuingInstitute_Id" });
            DropIndex("dbo.ERApplications", new[] { "ScreeningReport_Id" });
            DropIndex("dbo.ERApplications", new[] { "OperatorName_Id" });
            DropIndex("dbo.ERApplications", new[] { "FieldTypes_Id" });
            DropIndex("dbo.Comments", new[] { "LinkedUser_Id" });
            DropIndex("dbo.Comments", new[] { "LinkedApplication_Id" });
            DropTable("dbo.Notifications");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.ERScreeningInstitutes");
            DropTable("dbo.ERScreeningDetails");
            DropTable("dbo.Operators");
            DropTable("dbo.FieldTypes");
            DropTable("dbo.ERApplications");
            DropTable("dbo.Comments");
        }
    }
}
