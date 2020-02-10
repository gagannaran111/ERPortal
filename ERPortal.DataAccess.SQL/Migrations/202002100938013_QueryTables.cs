namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueryTables : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.QueryDetails", "UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.QueryMasters", "QueryRefId", "dbo.QueryDetails");
            DropIndex("dbo.QueryDetails", new[] { "UserAccountId" });
            DropIndex("dbo.QueryMasters", new[] { "QueryRefId" });
            AddColumn("dbo.QueryDetails", "CommentRefId", c => c.String());
            AddColumn("dbo.QueryDetails", "QueryParentId", c => c.String());
            AddColumn("dbo.QueryDetails", "Status", c => c.String());
            AddColumn("dbo.QueryMasters", "ERApplicationId", c => c.String());
            AddColumn("dbo.QueryMasters", "Subject", c => c.String());
            AddColumn("dbo.QueryMasters", "QueryParentId", c => c.String());
            DropColumn("dbo.QueryDetails", "ERApplicationId");
            DropColumn("dbo.QueryDetails", "Subject");
            DropColumn("dbo.QueryDetails", "Text");
            DropColumn("dbo.QueryDetails", "QueryParent");
            DropColumn("dbo.QueryDetails", "UserAccountId");
            DropColumn("dbo.QueryMasters", "Sender");
            DropColumn("dbo.QueryMasters", "Reciever");
            DropColumn("dbo.QueryMasters", "QueryRefId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.QueryMasters", "QueryRefId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.QueryMasters", "Reciever", c => c.String());
            AddColumn("dbo.QueryMasters", "Sender", c => c.String());
            AddColumn("dbo.QueryDetails", "UserAccountId", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.QueryDetails", "QueryParent", c => c.String());
            AddColumn("dbo.QueryDetails", "Text", c => c.String());
            AddColumn("dbo.QueryDetails", "Subject", c => c.String());
            AddColumn("dbo.QueryDetails", "ERApplicationId", c => c.String());
            DropColumn("dbo.QueryMasters", "QueryParentId");
            DropColumn("dbo.QueryMasters", "Subject");
            DropColumn("dbo.QueryMasters", "ERApplicationId");
            DropColumn("dbo.QueryDetails", "Status");
            DropColumn("dbo.QueryDetails", "QueryParentId");
            DropColumn("dbo.QueryDetails", "CommentRefId");
            CreateIndex("dbo.QueryMasters", "QueryRefId");
            CreateIndex("dbo.QueryDetails", "UserAccountId");
            AddForeignKey("dbo.QueryMasters", "QueryRefId", "dbo.QueryDetails", "Id", cascadeDelete: true);
            AddForeignKey("dbo.QueryDetails", "UserAccountId", "dbo.UserAccounts", "Id", cascadeDelete: true);
        }
    }
}
