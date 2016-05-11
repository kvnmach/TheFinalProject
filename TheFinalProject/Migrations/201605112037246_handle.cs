namespace TheFinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class handle : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Handle", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Handle");
        }
    }
}
