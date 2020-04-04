namespace CarRentalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CarModels", "Id", c => c.Guid(nullable: false));
            CreateIndex("dbo.CarModels", "Id");
            AddForeignKey("dbo.CarModels", "Id", "dbo.RentalCatergories", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CarModels", "Id", "dbo.RentalCatergories");
            DropIndex("dbo.CarModels", new[] { "Id" });
            DropColumn("dbo.CarModels", "Id");
        }
    }
}
