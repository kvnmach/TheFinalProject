namespace TheFinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class follow : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Followings", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.AspNetUsers", "ApplicationUser_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.AspNetUsers", "ApplicationUser_Id");
            DropTable("dbo.Followings");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Followings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            DropIndex("dbo.AspNetUsers", new[] { "ApplicationUser_Id" });
            DropColumn("dbo.AspNetUsers", "ApplicationUser_Id");
            CreateIndex("dbo.Followings", "ApplicationUser_Id");
        }
    }
}
