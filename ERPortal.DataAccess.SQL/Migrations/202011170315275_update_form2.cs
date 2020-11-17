namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_form2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERApplications", "MassiveFracking", c => c.Int());
            AlterColumn("dbo.ERApplications", "PresentlyUnderProduction", c => c.Int());
            AlterColumn("dbo.ERApplications", "FieldOIIP", c => c.String());
            AlterColumn("dbo.ERApplications", "FieldGIIP", c => c.String());
            AlterColumn("dbo.ERApplications", "EUR", c => c.String());
            AlterColumn("dbo.ERApplications", "Np", c => c.String());
            AlterColumn("dbo.ERApplications", "CurrentRecovery", c => c.String());
            AlterColumn("dbo.ERApplications", "ERTechniquesType", c => c.Int());
            AlterColumn("dbo.ERPilots", "CAPEX", c => c.String());
            AlterColumn("dbo.ERPilots", "OPEX", c => c.String());
            AlterColumn("dbo.ERPilots", "Duration", c => c.Int());
            AlterColumn("dbo.ERPilots", "ExpectedInvestment", c => c.String());
            AlterColumn("dbo.ERPilots", "EnvisagedIncremental", c => c.String());
            AlterColumn("dbo.ERPilots", "CAPEX2", c => c.String());
            AlterColumn("dbo.ERPilots", "OPEX2", c => c.String());
            AlterColumn("dbo.ERPilots", "ExpectedInvestment2", c => c.String());
            AlterColumn("dbo.ERPilots", "EnvisagedIncremental2", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERPilots", "EnvisagedIncremental2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "ExpectedInvestment2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "OPEX2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "CAPEX2", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "EnvisagedIncremental", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "ExpectedInvestment", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "Duration", c => c.Int(nullable: false));
            AlterColumn("dbo.ERPilots", "OPEX", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERPilots", "CAPEX", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "ERTechniquesType", c => c.Int(nullable: false));
            AlterColumn("dbo.ERApplications", "CurrentRecovery", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "Np", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "EUR", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "FieldGIIP", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "FieldOIIP", c => c.Decimal(precision: 18, scale: 2));
            AlterColumn("dbo.ERApplications", "PresentlyUnderProduction", c => c.Int(nullable: false));
            AlterColumn("dbo.ERApplications", "MassiveFracking", c => c.Int(nullable: false));
        }
    }
}
