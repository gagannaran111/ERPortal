namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gagan1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERScreeningDetails", "ReportDocumentPath", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERScreeningDetails", "ReportDocumentPath", c => c.String());
        }
    }
}
