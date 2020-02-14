namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueryTableAddColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QueryDetails", "CustQueryId", c => c.String());
            AddColumn("dbo.QueryMasters", "CustQueryId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QueryMasters", "CustQueryId");
            DropColumn("dbo.QueryDetails", "CustQueryId");
        }
    }
}
