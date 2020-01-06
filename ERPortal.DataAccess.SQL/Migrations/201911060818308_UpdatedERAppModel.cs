namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedERAppModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ERApplications", "FieldName", c => c.String(nullable: false));
            AlterColumn("dbo.ERApplications", "ImplementaionType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ERApplications", "ImplementaionType", c => c.Int());
            AlterColumn("dbo.ERApplications", "FieldName", c => c.String());
        }
    }
}
