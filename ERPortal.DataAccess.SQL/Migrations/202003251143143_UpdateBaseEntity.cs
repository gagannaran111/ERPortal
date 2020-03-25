namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateBaseEntity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AuditTrails", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.UserAccounts", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.UserAccounts", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.DepartmentTypes", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Organisations", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Organisations", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.Comments", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Comments", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.ERApplications", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ERPilots", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ERScreeningDetails", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ERScreeningDetails", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.ERScreeningInstitutes", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ERScreeningInstitutes", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.FieldTypes", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.FieldTypes", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.UHCProductionMethods", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.UHCProductionMethods", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.ERAppActiveUsers", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ErrorLogs", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ERTechniques", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.ForwardApplications", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Notifications", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.Notifications", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.QueryDetails", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.QueryMasters", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.QueryUsers", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.QueryUsers", "Is_Active", c => c.Boolean(nullable: false));
            AddColumn("dbo.StatusMasters", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.UploadFiles", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("dbo.UploadFiles", "Is_Active", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadFiles", "Is_Active");
            DropColumn("dbo.UploadFiles", "ModifiedAt");
            DropColumn("dbo.StatusMasters", "ModifiedAt");
            DropColumn("dbo.QueryUsers", "Is_Active");
            DropColumn("dbo.QueryUsers", "ModifiedAt");
            DropColumn("dbo.QueryMasters", "ModifiedAt");
            DropColumn("dbo.QueryDetails", "ModifiedAt");
            DropColumn("dbo.Notifications", "Is_Active");
            DropColumn("dbo.Notifications", "ModifiedAt");
            DropColumn("dbo.ForwardApplications", "ModifiedAt");
            DropColumn("dbo.ERTechniques", "ModifiedAt");
            DropColumn("dbo.ErrorLogs", "ModifiedAt");
            DropColumn("dbo.ERAppActiveUsers", "ModifiedAt");
            DropColumn("dbo.UHCProductionMethods", "Is_Active");
            DropColumn("dbo.UHCProductionMethods", "ModifiedAt");
            DropColumn("dbo.FieldTypes", "Is_Active");
            DropColumn("dbo.FieldTypes", "ModifiedAt");
            DropColumn("dbo.ERScreeningInstitutes", "Is_Active");
            DropColumn("dbo.ERScreeningInstitutes", "ModifiedAt");
            DropColumn("dbo.ERScreeningDetails", "Is_Active");
            DropColumn("dbo.ERScreeningDetails", "ModifiedAt");
            DropColumn("dbo.ERPilots", "ModifiedAt");
            DropColumn("dbo.ERApplications", "ModifiedAt");
            DropColumn("dbo.Comments", "Is_Active");
            DropColumn("dbo.Comments", "ModifiedAt");
            DropColumn("dbo.Organisations", "Is_Active");
            DropColumn("dbo.Organisations", "ModifiedAt");
            DropColumn("dbo.DepartmentTypes", "ModifiedAt");
            DropColumn("dbo.UserAccounts", "Is_Active");
            DropColumn("dbo.UserAccounts", "ModifiedAt");
            DropColumn("dbo.AuditTrails", "ModifiedAt");
        }
    }
}
