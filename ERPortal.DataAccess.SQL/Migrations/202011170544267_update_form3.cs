namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_form3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERPilots", "PilotDesign", c => c.Int());
            AlterColumn("dbo.ERScreeningDetails", "FirstOrderScreening", c => c.Int());
            AlterColumn("dbo.ERScreeningDetails", "SecondOrderScreening", c => c.Int());
            AlterColumn("dbo.ERScreeningDetails", "ThirdOrderScreening", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERScreeningDetails", "ThirdOrderScreening", c => c.Int(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "SecondOrderScreening", c => c.Int(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "FirstOrderScreening", c => c.Int(nullable: false));
            AlterColumn("dbo.ERPilots", "PilotDesign", c => c.Int(nullable: false));
        }
    }
}
