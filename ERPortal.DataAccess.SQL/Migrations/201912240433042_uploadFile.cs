namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadFile : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UploadFiles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FIleRef = c.String(),
                        FileName = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UploadFiles");
        }
    }
}
