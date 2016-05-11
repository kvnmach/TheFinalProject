namespace TheFinalProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class location : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tools", "ZipCode", c => c.String());
            AddColumn("dbo.Tools", "City", c => c.String());
            AddColumn("dbo.Tools", "State", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tools", "State");
            DropColumn("dbo.Tools", "City");
            DropColumn("dbo.Tools", "ZipCode");
        }
    }
}
