namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class uploadFile1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UploadFiles", "FilePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadFiles", "FilePath");
        }
    }
}
