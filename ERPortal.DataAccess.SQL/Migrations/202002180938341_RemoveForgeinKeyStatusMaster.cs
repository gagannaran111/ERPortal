namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveForgeinKeyStatusMaster : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AuditTrails", "StatusId", "dbo.StatusMasters");
            DropIndex("dbo.AuditTrails", new[] { "StatusId" });
            AlterColumn("dbo.AuditTrails", "StatusId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AuditTrails", "StatusId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.AuditTrails", "StatusId");
            AddForeignKey("dbo.AuditTrails", "StatusId", "dbo.StatusMasters", "Id");
        }
    }
}
