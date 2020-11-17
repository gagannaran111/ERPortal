namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class update_form32 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERApplications", "HydrocarbonMethod", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERApplications", "HydrocarbonMethod", c => c.Int(nullable: false));
        }
    }
}
