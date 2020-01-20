namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubDateOfPilotReport : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ERApplications", "DGHPilotReportSubmissionDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ERApplications", "DGHPilotReportSubmissionDate");
        }
    }
}
