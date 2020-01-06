namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ERScreeningDetailTableUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERScreeningDetails", "DateOfSubmission", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERScreeningDetails", "DateOfSubmission", c => c.DateTime(nullable: true));
        }
    }
}
