namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QueryTables2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QueryUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        SenderId = c.String(),
                        RecieverId = c.String(),
                        QueryId = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.QueryUsers");
        }
    }
}
