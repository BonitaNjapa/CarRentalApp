using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using CarRentalApp.Models;
using CarRentalApp.Repository.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRentalApp.Repository
{
    public class Employees : IEmployees
    {
        public async System.Threading.Tasks.Task<Results> AddNewEmployees(EmployeeViewModel employeeViewModel)
        {
            var res = new Results();
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = new ApplicationUser { UserName = employeeViewModel.Email, Email = employeeViewModel.Email, FirstName = employeeViewModel.FirstName, LastName = employeeViewModel.LastName, Contact = employeeViewModel.Contact };
                var result = await userManager.CreateAsync(user, "Password01");
                if (result.Succeeded)
                {
                    var role = roleManager.FindById(employeeViewModel.Role);
                    if(role != null)
                    {
                        userManager.AddToRole(user.Id, role.Name);
                    }
                    res.IsCompletedSuccessFully = true;
                    res.Errors = null;
                }
                else
                {
                    res.IsCompletedSuccessFully = false;
                    res.Errors = result.Errors.ToList();
                }
            }
            return res;
        }

        public int EmpCount()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Users.Count();
            }
        }

        public ICollection<EmployeeViewModel> GetAllEmployees()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var employees = new List<EmployeeViewModel>();
                foreach(var item in context.Users.Include(m => m.Roles).OrderBy(m => m.FirstName).ToList())
                {
                    var roleName = roleManager.FindById(item.Roles.FirstOrDefault().RoleId)?.Name;
                    employees.Add(new EmployeeViewModel()
                    {
                        UserId = item.Id,
                        FirstName = item.FirstName,
                        LastName = item.LastName,
                        Contact = item.Contact,
                        Email = item.Email,
                        Role = roleName 
                    });
                }

                return employees.Where(m => m.Role != "Admin" && m.Role != "Customer").ToList();
            }
        }

        public ICollection<RolesViewModel> GetAllRoles()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var roles = roleManager.Roles;
                var rolesFound = new List<RolesViewModel>();

                foreach(var item in roles)
                {
                    rolesFound.Add(new RolesViewModel()
                    {
                        RoleId = item.Id,
                        RoleName = item.Name
                    });
                }

                return rolesFound.Where(m => m.RoleName != "Admin").ToList();
            }
        }
    }
}