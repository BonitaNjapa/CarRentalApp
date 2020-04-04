namespace CarRentalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RentalItems", "CarModelId", c => c.Guid(nullable: false));
            CreateIndex("dbo.RentalItems", "CarModelId");
            AddForeignKey("dbo.RentalItems", "CarModelId", "dbo.CarModels", "CarModelId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RentalItems", "CarModelId", "dbo.CarModels");
            DropIndex("dbo.RentalItems", new[] { "CarModelId" });
            DropColumn("dbo.RentalItems", "CarModelId");
        }
    }
}
