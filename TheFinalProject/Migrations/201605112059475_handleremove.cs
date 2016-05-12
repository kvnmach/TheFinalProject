namespace TheFinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class handleremove : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "Handle");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Handle", c => c.String());
        }
    }
}
