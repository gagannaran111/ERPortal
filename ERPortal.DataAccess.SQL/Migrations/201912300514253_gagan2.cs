namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gagan2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERScreeningDetails", "ReportDocumentPath", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERScreeningDetails", "ReportDocumentPath", c => c.String(nullable: false));
        }
    }
}
