namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnUpdates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Subject", c => c.String());
            AddColumn("dbo.ERScreeningDetails", "FirstOrderScrText", c => c.String());
            AddColumn("dbo.ERScreeningDetails", "SecondOrderScrText", c => c.String());
            AddColumn("dbo.ERScreeningDetails", "ThirdOrderScrText", c => c.String());
            AddColumn("dbo.UploadFiles", "NewFileName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UploadFiles", "NewFileName");
            DropColumn("dbo.ERScreeningDetails", "ThirdOrderScrText");
            DropColumn("dbo.ERScreeningDetails", "SecondOrderScrText");
            DropColumn("dbo.ERScreeningDetails", "FirstOrderScrText");
            DropColumn("dbo.Comments", "Subject");
        }
    }
}
