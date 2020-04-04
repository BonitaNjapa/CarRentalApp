namespace CarRentalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Street = c.String(),
                        Province = c.String(),
                        City = c.String(),
                        Suburb = c.String(),
                        Code = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Extras",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        ImageUri = c.String(),
                        Cancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Branches",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        telephone = c.String(),
                        Email = c.String(),
                        address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.CarModels",
                c => new
                    {
                        CarModelId = c.Guid(nullable: false),
                        Name = c.String(),
                        Year = c.String(),
                        CarModelCount = c.Int(nullable: false),
                        ImageUrl = c.String(),
                        variant = c.String(),
                        ManufactureId = c.Guid(nullable: false),
                        catId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.CarModelId)
                .ForeignKey("dbo.Catergories", t => t.catId, cascadeDelete: true)
                .ForeignKey("dbo.Manufactures", t => t.ManufactureId, cascadeDelete: true)
                .Index(t => t.ManufactureId)
                .Index(t => t.catId);
            
            CreateTable(
                "dbo.Catergories",
                c => new
                    {
                        catId = c.Guid(nullable: false),
                        catName = c.String(),
                    })
                .PrimaryKey(t => t.catId);
            
            CreateTable(
                "dbo.Manufactures",
                c => new
                    {
                        ManufactureId = c.Guid(nullable: false),
                        Name = c.String(),
                        Logo = c.String(),
                    })
                .PrimaryKey(t => t.ManufactureId);
            
            CreateTable(
                "dbo.Drivers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FirstName = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        IdNumber = c.String(nullable: false),
                        LicenceNumber = c.String(nullable: false),
                        IDcopy = c.String(nullable: false),
                        LicenceCopy = c.String(nullable: false),
                        BookingId = c.Guid(nullable: false),
                        Approved = c.Boolean(nullable: false),
                        Cancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ExtrasBookings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ExtraId = c.Guid(nullable: false),
                        BookingId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Insurances",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Price = c.Double(nullable: false),
                        Description = c.String(),
                        Cancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.InsuranceBookings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        InsuranceId = c.Guid(nullable: false),
                        BookingId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.RentalItems", "Extras_Id", c => c.Guid());
            AddColumn("dbo.Locations", "ContactPerson", c => c.String());
            AddColumn("dbo.Locations", "Contact", c => c.String());
            AddColumn("dbo.Locations", "Cancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.Locations", "Address_Id", c => c.Guid());
            CreateIndex("dbo.RentalItems", "Extras_Id");
            CreateIndex("dbo.Locations", "Address_Id");
            AddForeignKey("dbo.RentalItems", "Extras_Id", "dbo.Extras", "Id");
            AddForeignKey("dbo.Locations", "Address_Id", "dbo.Addresses", "Id");
            DropColumn("dbo.BookingLocations", "BookingId");
            DropColumn("dbo.RentalItems", "Extras_HDW");
            DropColumn("dbo.RentalItems", "Extras_GW");
            DropColumn("dbo.RentalItems", "Extras_MDW");
            DropColumn("dbo.RentalItems", "Extras_Tyre_Waiver");
            DropColumn("dbo.RentalItems", "Extras_Child_Seat");
            DropColumn("dbo.RentalItems", "Extras_Bike_Carrier");
            DropColumn("dbo.RentalItems", "Extras_GPS");
            DropColumn("dbo.RentalItems", "Extras_Child_S");
            DropColumn("dbo.RentalItems", "Extras_Child_Se");
            DropColumn("dbo.RentalItems", "Extras_Add_Driver");
            DropColumn("dbo.RentalItems", "Extras_Fuel_Options");
            DropColumn("dbo.Locations", "Telephone");
            DropColumn("dbo.Locations", "Address");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Locations", "Address", c => c.String());
            AddColumn("dbo.Locations", "Telephone", c => c.String());
            AddColumn("dbo.RentalItems", "Extras_Fuel_Options", c => c.String());
            AddColumn("dbo.RentalItems", "Extras_Add_Driver", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_Child_Se", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_Child_S", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_GPS", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_Bike_Carrier", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_Child_Seat", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_Tyre_Waiver", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_MDW", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_GW", c => c.Boolean(nullable: false));
            AddColumn("dbo.RentalItems", "Extras_HDW", c => c.Boolean(nullable: false));
            AddColumn("dbo.BookingLocations", "BookingId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.CarModels", "ManufactureId", "dbo.Manufactures");
            DropForeignKey("dbo.CarModels", "catId", "dbo.Catergories");
            DropForeignKey("dbo.Locations", "Address_Id", "dbo.Addresses");
            DropForeignKey("dbo.RentalItems", "Extras_Id", "dbo.Extras");
            DropIndex("dbo.CarModels", new[] { "catId" });
            DropIndex("dbo.CarModels", new[] { "ManufactureId" });
            DropIndex("dbo.Locations", new[] { "Address_Id" });
            DropIndex("dbo.RentalItems", new[] { "Extras_Id" });
            DropColumn("dbo.Locations", "Address_Id");
            DropColumn("dbo.Locations", "Cancelled");
            DropColumn("dbo.Locations", "Contact");
            DropColumn("dbo.Locations", "ContactPerson");
            DropColumn("dbo.RentalItems", "Extras_Id");
            DropTable("dbo.InsuranceBookings");
            DropTable("dbo.Insurances");
            DropTable("dbo.ExtrasBookings");
            DropTable("dbo.Drivers");
            DropTable("dbo.Manufactures");
            DropTable("dbo.Catergories");
            DropTable("dbo.CarModels");
            DropTable("dbo.Branches");
            DropTable("dbo.Extras");
            DropTable("dbo.Addresses");
        }
    }
}
