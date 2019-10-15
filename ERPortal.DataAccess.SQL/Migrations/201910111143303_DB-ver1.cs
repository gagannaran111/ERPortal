namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBver1 : DbMigration
    {
        public override void Up()
        {
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
                        EligibleForFiscalIncentive = c.Boolean(),
                        DGHApprovalStatus = c.DateTime(),
                        DGHComments = c.String(),
                        DGHFileAttachment = c.String(),
                        PilotMandatory = c.Boolean(),
                        PilotReportApprovalStatus = c.Boolean(),
                        DGHCommentsForPilot = c.String(),
                        DGHFileAttachmentForPilot = c.String(),
                        FinalApprovalStatus = c.Boolean(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ScreeningReport_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERScreeningDetails", t => t.ScreeningReport_Id)
                .Index(t => t.ScreeningReport_Id);
            
            CreateTable(
                "dbo.ERCMemberComments",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Remarks = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        LinkedApplication_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.LinkedApplication_Id, cascadeDelete: true)
                .Index(t => t.LinkedApplication_Id);
            
            CreateTable(
                "dbo.FieldTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Type = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ERApplication_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.ERApplication_Id)
                .Index(t => t.ERApplication_Id);
            
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
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        EmailId = c.String(),
                        Password = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ERApplications", "ScreeningReport_Id", "dbo.ERScreeningDetails");
            DropForeignKey("dbo.ERScreeningDetails", "IssuingInstitute_Id", "dbo.ERScreeningInstitutes");
            DropForeignKey("dbo.FieldTypes", "ERApplication_Id", "dbo.ERApplications");
            DropForeignKey("dbo.ERCMemberComments", "LinkedApplication_Id", "dbo.ERApplications");
            DropIndex("dbo.ERScreeningDetails", new[] { "IssuingInstitute_Id" });
            DropIndex("dbo.FieldTypes", new[] { "ERApplication_Id" });
            DropIndex("dbo.ERCMemberComments", new[] { "LinkedApplication_Id" });
            DropIndex("dbo.ERApplications", new[] { "ScreeningReport_Id" });
            DropTable("dbo.UserAccounts");
            DropTable("dbo.ERScreeningInstitutes");
            DropTable("dbo.ERScreeningDetails");
            DropTable("dbo.FieldTypes");
            DropTable("dbo.ERCMemberComments");
            DropTable("dbo.ERApplications");
        }
    }
}
