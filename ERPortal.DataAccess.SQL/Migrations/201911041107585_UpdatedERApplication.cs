namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdatedERApplication : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UHCProductionMethods",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Description = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ERApplications", "HydrocarbonType", c => c.Int(nullable: false));
            AddColumn("dbo.ERApplications", "UHCProductionMethodId", c => c.String(maxLength: 128));
            AddColumn("dbo.ERApplications", "ImplementaionType", c => c.Int());
            CreateIndex("dbo.ERApplications", "UHCProductionMethodId");
            AddForeignKey("dbo.ERApplications", "UHCProductionMethodId", "dbo.UHCProductionMethods", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ERApplications", "UHCProductionMethodId", "dbo.UHCProductionMethods");
            DropIndex("dbo.ERApplications", new[] { "UHCProductionMethodId" });
            DropColumn("dbo.ERApplications", "ImplementaionType");
            DropColumn("dbo.ERApplications", "UHCProductionMethodId");
            DropColumn("dbo.ERApplications", "HydrocarbonType");
            DropTable("dbo.UHCProductionMethods");
        }
    }
}
