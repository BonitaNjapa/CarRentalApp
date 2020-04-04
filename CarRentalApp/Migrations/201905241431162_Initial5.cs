namespace CarRentalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalItems", "currentMileage", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RentalItems", "currentMileage");
        }
    }
}
