 namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AuditTrails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(),
                        FileRefId = c.String(),
                        StatusId = c.String(),
                        QueryDetailsId = c.String(),
                        SenderId = c.String(),
                        ReceiverId = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ERAppActiveUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(),
                        UserAccountId = c.String(nullable: false, maxLength: 128),
                        Dept_Id = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        Status = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccountId, cascadeDelete: true)
                .Index(t => t.UserAccountId);
            
            CreateTable(
                "dbo.ErrorLogs",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(),
                        PageUrl = c.String(),
                        ErrorText = c.String(),
                        UserAccountId = c.String(maxLength: 128),
                        Is_Active = c.Boolean(nullable: false),
                        ErrorStatus = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccountId)
                .Index(t => t.UserAccountId);
            
            CreateTable(
                "dbo.ForwardApplications",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(nullable: false, maxLength: 128),
                        Sender = c.String(),
                        Reciever = c.String(),
                        Subject = c.String(),
                        CommentRefId = c.String(),
                        FileRef = c.String(),
                        FileStatus = c.String(),
                        Is_active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ERApplications", t => t.ERApplicationId, cascadeDelete: true)
                .Index(t => t.ERApplicationId);
            
            CreateTable(
                "dbo.QueryDetails",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        ERApplicationId = c.String(),
                        Subject = c.String(),
                        Text = c.String(),
                        QueryParent = c.String(),
                        QuerySeq = c.Int(nullable: false),
                        FileRefId = c.String(),
                        UserAccountId = c.String(nullable: false, maxLength: 128),
                        Is_Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UserAccounts", t => t.UserAccountId, cascadeDelete: true)
                .Index(t => t.UserAccountId);
            
            CreateTable(
                "dbo.QueryMasters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Sender = c.String(),
                        Reciever = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        QueryRefId = c.String(nullable: false, maxLength: 128),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.QueryDetails", t => t.QueryRefId, cascadeDelete: true)
                .Index(t => t.QueryRefId);
            
            AddColumn("dbo.UserAccounts", "UserRole", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QueryMasters", "QueryRefId", "dbo.QueryDetails");
            DropForeignKey("dbo.QueryDetails", "UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.ForwardApplications", "ERApplicationId", "dbo.ERApplications");
            DropForeignKey("dbo.ErrorLogs", "UserAccountId", "dbo.UserAccounts");
            DropForeignKey("dbo.ERAppActiveUsers", "UserAccountId", "dbo.UserAccounts");
            DropIndex("dbo.QueryMasters", new[] { "QueryRefId" });
            DropIndex("dbo.QueryDetails", new[] { "UserAccountId" });
            DropIndex("dbo.ForwardApplications", new[] { "ERApplicationId" });
            DropIndex("dbo.ErrorLogs", new[] { "UserAccountId" });
            DropIndex("dbo.ERAppActiveUsers", new[] { "UserAccountId" });
            DropColumn("dbo.UserAccounts", "UserRole");
            DropTable("dbo.QueryMasters");
            DropTable("dbo.QueryDetails");
            DropTable("dbo.ForwardApplications");
            DropTable("dbo.ErrorLogs");
            DropTable("dbo.ERAppActiveUsers");
            DropTable("dbo.AuditTrails");
        }
    }
}
