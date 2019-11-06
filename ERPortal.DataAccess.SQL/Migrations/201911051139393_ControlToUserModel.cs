namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ControlToUserModel : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.UserAccounts", "LastName", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.UserAccounts", "LastName", c => c.String());
        }
    }
}
