namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveTypeInComment : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.AuditTrails", new[] { "StatusId" });
            AlterColumn("dbo.AuditTrails", "StatusId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AuditTrails", "SenderId", c => c.String(maxLength: 128));
            AlterColumn("dbo.AuditTrails", "ReceiverId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Comments", "Text", c => c.String(nullable: false));
            AlterColumn("dbo.ForwardApplications", "FileStatus", c => c.Int(nullable: false));
            CreateIndex("dbo.AuditTrails", "StatusId");
            CreateIndex("dbo.AuditTrails", "SenderId");
            CreateIndex("dbo.AuditTrails", "ReceiverId");
            AddForeignKey("dbo.AuditTrails", "ReceiverId", "dbo.UserAccounts", "Id");
            AddForeignKey("dbo.AuditTrails", "SenderId", "dbo.UserAccounts", "Id");
            DropColumn("dbo.Comments", "Type");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Comments", "Type", c => c.Int(nullable: false));
            DropForeignKey("dbo.AuditTrails", "SenderId", "dbo.UserAccounts");
            DropForeignKey("dbo.AuditTrails", "ReceiverId", "dbo.UserAccounts");
            DropIndex("dbo.AuditTrails", new[] { "ReceiverId" });
            DropIndex("dbo.AuditTrails", new[] { "SenderId" });
            DropIndex("dbo.AuditTrails", new[] { "StatusId" });
            AlterColumn("dbo.ForwardApplications", "FileStatus", c => c.String());
            AlterColumn("dbo.Comments", "Text", c => c.String());
            AlterColumn("dbo.AuditTrails", "ReceiverId", c => c.String());
            AlterColumn("dbo.AuditTrails", "SenderId", c => c.String());
            AlterColumn("dbo.AuditTrails", "StatusId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AuditTrails", "StatusId");
        }
    }
}
