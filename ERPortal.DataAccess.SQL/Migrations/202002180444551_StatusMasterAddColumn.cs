namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StatusMasterAddColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.StatusMasters", "CustStatusId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.StatusMasters", "CustStatusId");
        }
    }
}
