namespace CarRentalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalItems", "RegNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RentalItems", "RegNumber");
        }
    }
}
