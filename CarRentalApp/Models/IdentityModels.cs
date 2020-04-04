using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRentalApp.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Contact { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            //Database.SetInitializer<ApplicationDbContext>(null);
        }
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<VehicleMake> VehicleMakes { get; set; }
        public DbSet<RentalCatergory> RentalCatergory { get; set; }
        public DbSet<RentalItems> RentalItems { get; set; }
        public DbSet<Locations> Locations { get; set; }
        public DbSet<Bookings> Bookings { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<BookingLocation> BookingLocation { get; set; }
        //public System.Data.Entity.DbSet<CarRentalApp.Models.CMake> VehicleMakes { get; set; }
        //public System.Data.Entity.DbSet<CarRentalApp.Models.CModel> CModels { get; set; }

        public System.Data.Entity.DbSet<CarRentalApp.Models.GroupingClass> GroupingClasses { get; set; }

        public System.Data.Entity.DbSet<CarRentalApp.Models.Manufacture> Manufactures { get; set; }

        public System.Data.Entity.DbSet<CarRentalApp.Models.CarModel> CarModels { get; set; }

        public System.Data.Entity.DbSet<CarRentalApp.Models.Catergory> Catergories { get; set; }

  //      public System.Data.Entity.DbSet<CarRentalApp.Models.Car> Cars { get; set; }

        public System.Data.Entity.DbSet<CarRentalApp.Models.Branch> Branches { get; set; }
        public DbSet<Extras> Extras { get; set; }
        public DbSet<Address> Address { get; set; }
        public DbSet<Insurance> Insurance { get; set; }
        public DbSet<InsuranceBookings> InsuranceBookings { get; set; }
        public DbSet<ExtrasBooking> ExtrasBooking { get; set; }
        public DbSet<Driver> Driver { get; set; }
        public DbSet<Maintanance> Maintanances { get; set; }
        public DbSet<Damages> Damages { get; set; }

        public System.Data.Entity.DbSet<CarRentalApp.Models.CategoryPackage> CategoryPackages { get; set; }
        //public System.Data.Entity.DbSet<CarRentalApp.Models.GroupingClassWaiver> GroupingClassWaivers { get; set; }

    }

}