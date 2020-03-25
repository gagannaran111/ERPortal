namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppERScreningTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ERPilots",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        PilotDesign = c.Int(nullable: false),
                        PilotMandatory = c.Int(),
                        TechnicallyCompatible = c.Int(nullable: false),
                        PilotEconomicViability = c.Int(nullable: false),
                        PilotNPV = c.Int(nullable: false),
                        PilotIRR = c.Int(nullable: false),
                        PilotStartDate = c.DateTime(nullable: false),
                        PilotEndDate = c.DateTime(nullable: false),
                        FullFillEconomicViability = c.Int(),
                        FullFillNPV = c.Int(nullable: false),
                        FullFillIRR = c.Int(nullable: false),
                        FullFillStartDate = c.DateTime(nullable: false),
                        FullFillEndDate = c.DateTime(nullable: false),
                        EligibleForFiscalIncentive = c.Int(),
                        Is_Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ERTechniques",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Method = c.Int(nullable: false),
                        TechniqueType = c.String(nullable: false),
                        TechniqueName = c.String(nullable: false),
                        Status = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ERApplications", "HydrocarbonMethod", c => c.Int(nullable: false));
            AddColumn("dbo.ERApplications", "ERPilotId", c => c.String(maxLength: 128));
            AddColumn("dbo.ERApplications", "Is_Active", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ERApplications", "PresentlyUnderProduction", c => c.Int());
            AlterColumn("dbo.ERScreeningDetails", "FirstOrderScreening", c => c.Int(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "SecondOrderScreening", c => c.Int(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "ThirdOrderScreening", c => c.Int(nullable: false));
            CreateIndex("dbo.ERApplications", "ERPilotId");
            AddForeignKey("dbo.ERApplications", "ERPilotId", "dbo.ERPilots", "Id");
            DropColumn("dbo.ERApplications", "ERScreeningStatus");
            DropColumn("dbo.ERApplications", "PilotDesign");
            DropColumn("dbo.ERApplications", "PilotProductionProfile");
            DropColumn("dbo.ERApplications", "TechnicallyCompatible");
            DropColumn("dbo.ERApplications", "EconomicViability");
            DropColumn("dbo.ERApplications", "EligibleForFiscalIncentive");
            DropColumn("dbo.ERApplications", "DGHApprovalStatus");
            DropColumn("dbo.ERApplications", "DGHApprovalDate");
            DropColumn("dbo.ERApplications", "DGHFileAttachment");
            DropColumn("dbo.ERApplications", "PilotMandatory");
            DropColumn("dbo.ERApplications", "DGHPilotReportSubmissionDate");
            DropColumn("dbo.ERApplications", "PilotReportApprovalStatus");
            DropColumn("dbo.ERApplications", "DGHFileAttachmentForPilot");
            DropColumn("dbo.ERApplications", "FinalApprovalStatus");
            DropColumn("dbo.ERScreeningDetails", "ApprovalStatus");
            DropColumn("dbo.ERScreeningDetails", "DateOfSubmission");
            DropColumn("dbo.ERScreeningDetails", "DateOfLastApproval");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ERScreeningDetails", "DateOfLastApproval", c => c.DateTime());
            AddColumn("dbo.ERScreeningDetails", "DateOfSubmission", c => c.DateTime());
            AddColumn("dbo.ERScreeningDetails", "ApprovalStatus", c => c.Boolean());
            AddColumn("dbo.ERApplications", "FinalApprovalStatus", c => c.Boolean());
            AddColumn("dbo.ERApplications", "DGHFileAttachmentForPilot", c => c.String());
            AddColumn("dbo.ERApplications", "PilotReportApprovalStatus", c => c.Boolean());
            AddColumn("dbo.ERApplications", "DGHPilotReportSubmissionDate", c => c.DateTime());
            AddColumn("dbo.ERApplications", "PilotMandatory", c => c.Boolean());
            AddColumn("dbo.ERApplications", "DGHFileAttachment", c => c.String());
            AddColumn("dbo.ERApplications", "DGHApprovalDate", c => c.DateTime());
            AddColumn("dbo.ERApplications", "DGHApprovalStatus", c => c.Boolean());
            AddColumn("dbo.ERApplications", "EligibleForFiscalIncentive", c => c.Boolean());
            AddColumn("dbo.ERApplications", "EconomicViability", c => c.Boolean());
            AddColumn("dbo.ERApplications", "TechnicallyCompatible", c => c.Boolean());
            AddColumn("dbo.ERApplications", "PilotProductionProfile", c => c.Int());
            AddColumn("dbo.ERApplications", "PilotDesign", c => c.Boolean());
            AddColumn("dbo.ERApplications", "ERScreeningStatus", c => c.Boolean());
            DropForeignKey("dbo.ERApplications", "ERPilotId", "dbo.ERPilots");
            DropIndex("dbo.ERApplications", new[] { "ERPilotId" });
            AlterColumn("dbo.ERScreeningDetails", "ThirdOrderScreening", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "SecondOrderScreening", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "FirstOrderScreening", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ERApplications", "PresentlyUnderProduction", c => c.Boolean());
            DropColumn("dbo.ERApplications", "Is_Active");
            DropColumn("dbo.ERApplications", "ERPilotId");
            DropColumn("dbo.ERApplications", "HydrocarbonMethod");
            DropTable("dbo.ERTechniques");
            DropTable("dbo.ERPilots");
        }
    }
}
