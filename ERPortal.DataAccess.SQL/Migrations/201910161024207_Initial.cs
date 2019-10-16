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
                        Remarks = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LinkedUser_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.LinkedUser_Id, cascadeDelete: true)
                .Index(t => t.LinkedUser_Id);
            
            CreateTable(
                "dbo.ERApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FieldName = c.String(),
                        DateOfDiscovery = c.DateTime(nullable: false),
                        DateOfCommercialProduction = c.DateTime(),
                        PresentlyUnderProduction = c.Boolean(),
                        ERScreeningStatus = c.Boolean(),
                        FieldOIIP = c.Int(),
                        FieldGIIP = c.Int(),
                        PilotDesign = c.Boolean(),
                        PilotProductionProfile = c.String(),
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
                        FieldTypes_Id = c.String(maxLength: 128),
                        OperatorName_Id = c.String(nullable: false, maxLength: 128),
                        ScreeningReport_Id = c.String(maxLength: 128),
                        DGHComments_Id = c.String(maxLength: 128),
                        DGHCommentsForPilot_Id = c.String(maxLength: 128),
                        ERCComments_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FieldTypes", t => t.FieldTypes_Id)
                .ForeignKey("dbo.Operators", t => t.OperatorName_Id, cascadeDelete: true)
                .ForeignKey("dbo.ERScreeningDetails", t => t.ScreeningReport_Id)
                .ForeignKey("dbo.Comments", t => t.DGHComments_Id)
                .ForeignKey("dbo.Comments", t => t.DGHCommentsForPilot_Id)
                .ForeignKey("dbo.Comments", t => t.ERCComments_Id)
                .Index(t => t.FieldTypes_Id)
                .Index(t => t.OperatorName_Id)
                .Index(t => t.ScreeningReport_Id)
                .Index(t => t.DGHComments_Id)
                .Index(t => t.DGHCommentsForPilot_Id)
                .Index(t => t.ERCComments_Id);
            
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
                .ForeignKey("dbo.Operators", t => t.OperatorName_Id)
                .Index(t => t.OperatorName_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "LinkedUser_Id", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "OperatorName_Id", "dbo.Operators");
            DropForeignKey("dbo.ERApplications", "ERCComments_Id", "dbo.Comments");
            DropForeignKey("dbo.ERApplications", "DGHCommentsForPilot_Id", "dbo.Comments");
            DropForeignKey("dbo.ERApplications", "DGHComments_Id", "dbo.Comments");
            DropForeignKey("dbo.ERApplications", "ScreeningReport_Id", "dbo.ERScreeningDetails");
            DropForeignKey("dbo.ERScreeningDetails", "IssuingInstitute_Id", "dbo.ERScreeningInstitutes");
            DropForeignKey("dbo.ERApplications", "OperatorName_Id", "dbo.Operators");
            DropForeignKey("dbo.ERApplications", "FieldTypes_Id", "dbo.FieldTypes");
            DropIndex("dbo.UserAccounts", new[] { "OperatorName_Id" });
            DropIndex("dbo.ERScreeningDetails", new[] { "IssuingInstitute_Id" });
            DropIndex("dbo.ERApplications", new[] { "ERCComments_Id" });
            DropIndex("dbo.ERApplications", new[] { "DGHCommentsForPilot_Id" });
            DropIndex("dbo.ERApplications", new[] { "DGHComments_Id" });
            DropIndex("dbo.ERApplications", new[] { "ScreeningReport_Id" });
            DropIndex("dbo.ERApplications", new[] { "OperatorName_Id" });
            DropIndex("dbo.ERApplications", new[] { "FieldTypes_Id" });
            DropIndex("dbo.Comments", new[] { "LinkedUser_Id" });
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
