namespace CarRentalApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookingLocations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        PickUp = c.Boolean(nullable: false),
                        LocationId = c.Guid(nullable: false),
                        BookingId = c.Guid(nullable: false),
                        Bookings_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Bookings", t => t.Bookings_Id)
                .Index(t => t.Bookings_Id);
            
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Reference = c.String(nullable: false),
                        FirstName = c.String(nullable: false),
                        Surname = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        PhoneNumber = c.String(nullable: false),
                        PickUpDate = c.DateTime(nullable: false),
                        DropOffDate = c.DateTime(nullable: false),
                        PickUpTime = c.DateTime(nullable: false),
                        DropOffTime = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        TimeStamp = c.DateTime(nullable: false),
                        IsCheckedOut = c.Boolean(nullable: false),
                        IsCheckedIn = c.Boolean(nullable: false),
                        KmReading1 = c.Int(nullable: false),
                        AddKmReadingAfter = c.Int(nullable: false),
                        ExtrasCost = c.Double(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                        RentalItems_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .ForeignKey("dbo.RentalItems", t => t.RentalItems_Id)
                .Index(t => t.ApplicationUser_Id)
                .Index(t => t.RentalItems_Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Contact = c.String(),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.RentalItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ImageUrl = c.String(),
                        CarReg = c.String(),
                        VehicleName = c.String(),
                        Seats = c.Int(nullable: false),
                        Doors = c.Int(nullable: false),
                        AirCondition = c.Boolean(nullable: false),
                        TankCapacity = c.String(),
                        ManualOrAutomatic = c.String(),
                        MileagePerDay = c.Int(nullable: false),
                        IsUnlimited = c.Boolean(nullable: false),
                        Rate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsAvailable = c.Boolean(nullable: false),
                        IsBookable = c.Boolean(nullable: false),
                        Catergory = c.String(),
                        LocationName = c.String(),
                        GroupingClassName = c.String(),
                        VehicleMakeName = c.String(),
                        Extras_HDW = c.Boolean(nullable: false),
                        Extras_GW = c.Boolean(nullable: false),
                        Extras_MDW = c.Boolean(nullable: false),
                        Extras_Tyre_Waiver = c.Boolean(nullable: false),
                        Extras_Child_Seat = c.Boolean(nullable: false),
                        Extras_Bike_Carrier = c.Boolean(nullable: false),
                        Extras_GPS = c.Boolean(nullable: false),
                        Extras_Child_S = c.Boolean(nullable: false),
                        Extras_Child_Se = c.Boolean(nullable: false),
                        Extras_Add_Driver = c.Boolean(nullable: false),
                        Extras_Fuel_Options = c.String(),
                        CModel_Id = c.Guid(),
                        GroupingClass_ClassId = c.Guid(),
                        Location_Id = c.Guid(),
                        LocationsC_Id = c.Guid(),
                        RentalCatergory_Id = c.Guid(),
                        VehicleMake_VehicleMakeId = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.CModels", t => t.CModel_Id)
                .ForeignKey("dbo.GroupingClasses", t => t.GroupingClass_ClassId)
                .ForeignKey("dbo.Locations", t => t.Location_Id)
                .ForeignKey("dbo.Locations", t => t.LocationsC_Id)
                .ForeignKey("dbo.RentalCatergories", t => t.RentalCatergory_Id)
                .ForeignKey("dbo.VehicleMakes", t => t.VehicleMake_VehicleMakeId)
                .Index(t => t.CModel_Id)
                .Index(t => t.GroupingClass_ClassId)
                .Index(t => t.Location_Id)
                .Index(t => t.LocationsC_Id)
                .Index(t => t.RentalCatergory_Id)
                .Index(t => t.VehicleMake_VehicleMakeId);
            
            CreateTable(
                "dbo.CModels",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        VehicleModel = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Features",
                c => new
                    {
                        fId = c.Guid(nullable: false),
                        fName = c.String(),
                        description = c.String(),
                        faIcon = c.String(),
                        RentalItems_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.fId)
                .ForeignKey("dbo.RentalItems", t => t.RentalItems_Id)
                .Index(t => t.RentalItems_Id);
            
            CreateTable(
                "dbo.GroupingClasses",
                c => new
                    {
                        ClassId = c.Guid(nullable: false),
                        Name = c.String(),
                        StandardWaiver_WaiverCost = c.Double(nullable: false),
                        StandardWaiver_groupingClassId = c.Guid(nullable: false),
                        SuperWaiver_WaiverCost = c.Double(nullable: false),
                        SuperWaiver_groupingClassId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.ClassId);
            
            CreateTable(
                "dbo.Locations",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        Telephone = c.String(),
                        Email = c.String(),
                        Address = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RentalCatergories",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Category = c.String(nullable: false),
                        Cancelled = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.VehicleMakes",
                c => new
                    {
                        VehicleMakeId = c.Guid(nullable: false),
                        Make = c.String(),
                    })
                .PrimaryKey(t => t.VehicleMakeId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        TransactionDate = c.DateTime(nullable: false),
                        PayRequestId = c.String(),
                        Reference = c.String(),
                        Amount = c.Int(nullable: false),
                        EmailAddress = c.String(),
                        BookingId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Bookings", "RentalItems_Id", "dbo.RentalItems");
            DropForeignKey("dbo.RentalItems", "VehicleMake_VehicleMakeId", "dbo.VehicleMakes");
            DropForeignKey("dbo.RentalItems", "RentalCatergory_Id", "dbo.RentalCatergories");
            DropForeignKey("dbo.RentalItems", "LocationsC_Id", "dbo.Locations");
            DropForeignKey("dbo.RentalItems", "Location_Id", "dbo.Locations");
            DropForeignKey("dbo.RentalItems", "GroupingClass_ClassId", "dbo.GroupingClasses");
            DropForeignKey("dbo.Features", "RentalItems_Id", "dbo.RentalItems");
            DropForeignKey("dbo.RentalItems", "CModel_Id", "dbo.CModels");
            DropForeignKey("dbo.BookingLocations", "Bookings_Id", "dbo.Bookings");
            DropForeignKey("dbo.Bookings", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Features", new[] { "RentalItems_Id" });
            DropIndex("dbo.RentalItems", new[] { "VehicleMake_VehicleMakeId" });
            DropIndex("dbo.RentalItems", new[] { "RentalCatergory_Id" });
            DropIndex("dbo.RentalItems", new[] { "LocationsC_Id" });
            DropIndex("dbo.RentalItems", new[] { "Location_Id" });
            DropIndex("dbo.RentalItems", new[] { "GroupingClass_ClassId" });
            DropIndex("dbo.RentalItems", new[] { "CModel_Id" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.Bookings", new[] { "RentalItems_Id" });
            DropIndex("dbo.Bookings", new[] { "ApplicationUser_Id" });
            DropIndex("dbo.BookingLocations", new[] { "Bookings_Id" });
            DropTable("dbo.Transactions");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.VehicleMakes");
            DropTable("dbo.RentalCatergories");
            DropTable("dbo.Locations");
            DropTable("dbo.GroupingClasses");
            DropTable("dbo.Features");
            DropTable("dbo.CModels");
            DropTable("dbo.RentalItems");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Bookings");
            DropTable("dbo.BookingLocations");
        }
    }
}
