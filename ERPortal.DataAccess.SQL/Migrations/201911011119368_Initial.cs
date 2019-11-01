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
                        UserAccountId = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(nullable: false, maxLength: 128),
                        Type = c.Int(nullable: false),
                        Text = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.ERApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccountId, cascadeDelete: true)
                .Index(t => t.UserAccountId)
                .Index(t => t.ERApplicationId);
            
            CreateTable(
                "dbo.ERApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        OrganisationId = c.String(nullable: false, maxLength: 128),
                        FieldTypeId = c.String(nullable: false, maxLength: 128),
                        FieldName = c.String(),
                        DateOfDiscovery = c.DateTime(nullable: false),
                        DateOfInitialCommercialProduction = c.DateTime(),
                        DateOfLastCommercialProduction = c.DateTime(),
                        PresentlyUnderProduction = c.Boolean(),
                        ERScreeningStatus = c.Boolean(),
                        ERScreeningDetailId = c.String(maxLength: 128),
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERScreeningDetails", t => t.ERScreeningDetailId)
                .ForeignKey("dbo.FieldTypes", t => t.FieldTypeId)
                .ForeignKey("dbo.Organisations", t => t.OrganisationId)
                .Index(t => t.OrganisationId)
                .Index(t => t.FieldTypeId)
                .Index(t => t.ERScreeningDetailId);
            
            CreateTable(
                "dbo.ERScreeningDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ERScreeningInstituteId = c.String(maxLength: 128),
                        ReportDocumentPath = c.String(),
                        FirstOrderScreening = c.Boolean(),
                        SecondOrderScreening = c.Boolean(),
                        ThirdOrderScreening = c.Boolean(),
                        ApprovalStatus = c.Boolean(),
                        DateOfSubmission = c.DateTime(nullable: false),
                        DateOfLastApproval = c.DateTime(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERScreeningInstitutes", t => t.ERScreeningInstituteId)
                .Index(t => t.ERScreeningInstituteId);
            
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
                "dbo.FieldTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Organisations",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Type = c.Int(nullable: false),
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
                        OrganisationId = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organisations", t => t.OrganisationId)
                .Index(t => t.OrganisationId);
            
            CreateTable(
                "dbo.Notifications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserAccountId = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(maxLength: 128),
                        Text = c.String(),
                        IsRead = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.ERApplicationId, cascadeDelete: true)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccountId, cascadeDelete: true)
                .Index(t => t.UserAccountId)
                .Index(t => t.ERApplicationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Notifications", "UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.Notifications", "ERApplicationId", "dbo.ERApplications");
            DropForeignKey("dbo.Comments", "UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.UserAccounts", "OrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.Comments", "ERApplicationId", "dbo.ERApplications");
            DropForeignKey("dbo.ERApplications", "OrganisationId", "dbo.Organisations");
            DropForeignKey("dbo.ERApplications", "FieldTypeId", "dbo.FieldTypes");
            DropForeignKey("dbo.ERApplications", "ERScreeningDetailId", "dbo.ERScreeningDetails");
            DropForeignKey("dbo.ERScreeningDetails", "ERScreeningInstituteId", "dbo.ERScreeningInstitutes");
            DropIndex("dbo.Notifications", new[] { "ERApplicationId" });
            DropIndex("dbo.Notifications", new[] { "UserAccountId" });
            DropIndex("dbo.UserAccounts", new[] { "OrganisationId" });
            DropIndex("dbo.ERScreeningDetails", new[] { "ERScreeningInstituteId" });
            DropIndex("dbo.ERApplications", new[] { "ERScreeningDetailId" });
            DropIndex("dbo.ERApplications", new[] { "FieldTypeId" });
            DropIndex("dbo.ERApplications", new[] { "OrganisationId" });
            DropIndex("dbo.Comments", new[] { "ERApplicationId" });
            DropIndex("dbo.Comments", new[] { "UserAccountId" });
            DropTable("dbo.Notifications");
            DropTable("dbo.UserAccounts");
            DropTable("dbo.Organisations");
            DropTable("dbo.FieldTypes");
            DropTable("dbo.ERScreeningInstitutes");
            DropTable("dbo.ERScreeningDetails");
            DropTable("dbo.ERApplications");
            DropTable("dbo.Comments");
        }
    }
}
