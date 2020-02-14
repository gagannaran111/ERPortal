namespace ERPortal.DataAccess.SQL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DepartmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DepartmentTypes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        DeptName = c.String(),
                        SubDeptName = c.String(),
                        Is_Active = c.Boolean(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.UserAccounts", "DeptId", c => c.String(maxLength: 128));
            CreateIndex("dbo.UserAccounts", "DeptId");
            AddForeignKey("dbo.UserAccounts", "DeptId", "dbo.DepartmentTypes", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserAccounts", "DeptId", "dbo.DepartmentTypes");
            DropIndex("dbo.UserAccounts", new[] { "DeptId" });
            DropColumn("dbo.UserAccounts", "DeptId");
            DropTable("dbo.DepartmentTypes");
        }
    }
}
