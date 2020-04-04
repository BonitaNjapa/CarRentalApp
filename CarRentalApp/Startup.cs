using System;
using System.Linq;
using CarRentalApp.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CarRentalApp.Startup))]
namespace CarRentalApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolersandUsers();
           // AddLocations();
        }

        //private void AddLocations()
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    if (!db.Locations.Any())
        //    {
        //        Locations locations = new Locations()
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "King Shaka Airport(DUR)",
        //            Telephone = "031098765",
        //            Email = "SuzukaKingshaka@Suzuka.com",
        //            Address = "55 Ariel King Shaka"
        //        };
        //        db.Locations.Add(locations);

        //        Locations location2 = new Locations()
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "Durban City Center Branch",
        //            Telephone = "031098765",
        //            Email = "SuzukaUmngeni@Suzuka.com",
        //            Address = "55 Umngeni Road Durban"
        //        };
        //        db.Locations.Add(location2);

        //        db.SaveChanges();
        //    }
        //}

        private void CreateRolersandUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));


            if (!roleManager.RoleExists("Admin"))
            {
                roleManager.Create(new IdentityRole("Admin"));

                var user = new ApplicationUser();
                user.UserName = "admin@prodowner.com";
                user.Email = "admin@prodowner.com";
                string pwd = "Password01";

                var newuser = userManager.Create(user, pwd);
                if (newuser.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                }

            }
           
        }
        //private void SeedGroupings()
        //{
        //    ApplicationDbContext db = new ApplicationDbContext();
        //    if (!db.GroupingClasses.Any())
        //    {
        //        GroupingClass ClassA = new GroupingClass() { ClassId = new Guid(),s };
        //    }
        //}
    }
}
