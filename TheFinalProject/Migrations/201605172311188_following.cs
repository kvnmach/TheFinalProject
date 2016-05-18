namespace TheFinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class following : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Followings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Followings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Followings", new[] { "ApplicationUser_Id" });
            DropTable("dbo.Followings");
        }
    }
}
