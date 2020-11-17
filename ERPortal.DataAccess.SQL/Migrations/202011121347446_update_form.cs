namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_form : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ERApplications", "Area", c => c.String(nullable: false));
            AddColumn("dbo.ERApplications", "MassiveFracking", c => c.Int(nullable: false));
            AddColumn("dbo.ERApplications", "EUR", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ERApplications", "Np", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ERApplications", "CurrentRecovery", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.ERApplications", "ERTechniquesType", c => c.Int(nullable: false));
            AddColumn("dbo.ERPilots", "CAPEX", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "OPEX", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "Duration", c => c.Int(nullable: false));
            AddColumn("dbo.ERPilots", "ExpectedInvestment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "EnvisagedIncremental", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "CAPEX2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "OPEX2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "Duration2", c => c.Int(nullable: false));
            AddColumn("dbo.ERPilots", "ExpectedInvestment2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.ERPilots", "EnvisagedIncremental2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "FieldOIIP", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "FieldGIIP", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERTechniques", "TechniqueType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERTechniques", "TechniqueType", c => c.String(nullable: false));
            AlterColumn("dbo.ERApplications", "FieldGIIP", c => c.Int());
            AlterColumn("dbo.ERApplications", "FieldOIIP", c => c.Int());
            DropColumn("dbo.ERPilots", "EnvisagedIncremental2");
            DropColumn("dbo.ERPilots", "ExpectedInvestment2");
            DropColumn("dbo.ERPilots", "Duration2");
            DropColumn("dbo.ERPilots", "OPEX2");
            DropColumn("dbo.ERPilots", "CAPEX2");
            DropColumn("dbo.ERPilots", "EnvisagedIncremental");
            DropColumn("dbo.ERPilots", "ExpectedInvestment");
            DropColumn("dbo.ERPilots", "Duration");
            DropColumn("dbo.ERPilots", "OPEX");
            DropColumn("dbo.ERPilots", "CAPEX");
            DropColumn("dbo.ERApplications", "ERTechniquesType");
            DropColumn("dbo.ERApplications", "CurrentRecovery");
            DropColumn("dbo.ERApplications", "Np");
            DropColumn("dbo.ERApplications", "EUR");
            DropColumn("dbo.ERApplications", "MassiveFracking");
            DropColumn("dbo.ERApplications", "Area");
        }
    }
}
