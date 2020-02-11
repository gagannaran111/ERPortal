namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueryTables3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QueryDetails", "ERApplicationId", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.QueryDetails", "ERApplicationId");
        }
    }
}
