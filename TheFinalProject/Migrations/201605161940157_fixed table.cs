namespace TheFinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class fixedtable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id");
            AddForeignKey("dbo.AspNetUsers", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
