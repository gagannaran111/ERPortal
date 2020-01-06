namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gagan23 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ERApplications", "AppId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ERApplications", "AppId");
        }
    }
}
