namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewtable_StatusMaster : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StatusMasters",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Status = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AlterColumn("dbo.AuditTrails", "StatusId", c => c.String(maxLength: 128));
            CreateIndex("dbo.AuditTrails", "StatusId");
            AddForeignKey("dbo.AuditTrails", "StatusId", "dbo.StatusMasters", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AuditTrails", "StatusId", "dbo.StatusMasters");
            DropIndex("dbo.AuditTrails", new[] { "StatusId" });
            AlterColumn("dbo.AuditTrails", "StatusId", c => c.String());
            DropTable("dbo.StatusMasters");
        }
    }
}
