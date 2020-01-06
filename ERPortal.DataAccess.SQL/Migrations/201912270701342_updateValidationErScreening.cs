namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class updateValidationErScreening : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ERScreeningDetails", "ERScreeningInstituteId", "dbo.ERScreeningInstitutes");
            DropIndex("dbo.ERScreeningDetails", new[] { "ERScreeningInstituteId" });
            AlterColumn("dbo.ERScreeningDetails", "ERScreeningInstituteId", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.ERScreeningDetails", "FirstOrderScreening", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "SecondOrderScreening", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ERScreeningDetails", "ThirdOrderScreening", c => c.Boolean(nullable: false));
            CreateIndex("dbo.ERScreeningDetails", "ERScreeningInstituteId");
            AddForeignKey("dbo.ERScreeningDetails", "ERScreeningInstituteId", "dbo.ERScreeningInstitutes", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ERScreeningDetails", "ERScreeningInstituteId", "dbo.ERScreeningInstitutes");
            DropIndex("dbo.ERScreeningDetails", new[] { "ERScreeningInstituteId" });
            AlterColumn("dbo.ERScreeningDetails", "ThirdOrderScreening", c => c.Boolean());
            AlterColumn("dbo.ERScreeningDetails", "SecondOrderScreening", c => c.Boolean());
            AlterColumn("dbo.ERScreeningDetails", "FirstOrderScreening", c => c.Boolean());
            AlterColumn("dbo.ERScreeningDetails", "ERScreeningInstituteId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ERScreeningDetails", "ERScreeningInstituteId");
            AddForeignKey("dbo.ERScreeningDetails", "ERScreeningInstituteId", "dbo.ERScreeningInstitutes", "Id");
        }
    }
}
