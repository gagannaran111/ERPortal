namespace ERPortal.WebUI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ga : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "JobTitle", c => c.String());
            AddColumn("dbo.AspNetUsers", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "FullName");
            DropColumn("dbo.AspNetUsers", "JobTitle");
        }
    }
}
