namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ERTechniqueColAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ERApplications", "ERTechniquesId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AuditTrails", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.UserAccounts", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.DepartmentTypes", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Organisations", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Comments", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ERApplications", "PresentlyUnderProduction", c => c.Int(nullable: false));
            AlterColumn("dbo.ERApplications", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ERPilots", "PilotNPV", c => c.Int());
            AlterColumn("dbo.ERPilots", "PilotIRR", c => c.Int());
            AlterColumn("dbo.ERPilots", "PilotStartDate", c => c.DateTime());
            AlterColumn("dbo.ERPilots", "PilotEndDate", c => c.DateTime());
            AlterColumn("dbo.ERPilots", "FullFillNPV", c => c.Int());
            AlterColumn("dbo.ERPilots", "FullFillIRR", c => c.Int());
            AlterColumn("dbo.ERPilots", "FullFillStartDate", c => c.DateTime());
            AlterColumn("dbo.ERPilots", "FullFillEndDate", c => c.DateTime());
            AlterColumn("dbo.ERPilots", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ERScreeningDetails", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ERScreeningInstitutes", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.FieldTypes", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.UHCProductionMethods", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ERAppActiveUsers", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ErrorLogs", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ERTechniques", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.ForwardApplications", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.Notifications", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.QueryDetails", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.QueryMasters", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.QueryUsers", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.StatusMasters", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            AlterColumn("dbo.UploadFiles", "ModifiedAt", c => c.DateTimeOffset(precision: 7));
            CreateIndex("dbo.ERApplications", "ERTechniquesId");
            AddForeignKey("dbo.ERApplications", "ERTechniquesId", "dbo.ERTechniques", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ERApplications", "ERTechniquesId", "dbo.ERTechniques");
            DropIndex("dbo.ERApplications", new[] { "ERTechniquesId" });
            AlterColumn("dbo.UploadFiles", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.StatusMasters", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.QueryUsers", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.QueryMasters", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.QueryDetails", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Notifications", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ForwardApplications", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERTechniques", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ErrorLogs", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERAppActiveUsers", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.UHCProductionMethods", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.FieldTypes", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERScreeningInstitutes", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERScreeningDetails", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERPilots", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERPilots", "FullFillEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ERPilots", "FullFillStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ERPilots", "FullFillIRR", c => c.Int(nullable: false));
            AlterColumn("dbo.ERPilots", "FullFillNPV", c => c.Int(nullable: false));
            AlterColumn("dbo.ERPilots", "PilotEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ERPilots", "PilotStartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.ERPilots", "PilotIRR", c => c.Int(nullable: false));
            AlterColumn("dbo.ERPilots", "PilotNPV", c => c.Int(nullable: false));
            AlterColumn("dbo.ERApplications", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.ERApplications", "PresentlyUnderProduction", c => c.Int());
            AlterColumn("dbo.Comments", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.Organisations", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.DepartmentTypes", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.UserAccounts", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AlterColumn("dbo.AuditTrails", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("dbo.ERApplications", "ERTechniquesId");
        }
    }
}
