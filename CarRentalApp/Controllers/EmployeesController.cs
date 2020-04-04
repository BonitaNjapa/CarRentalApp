using CarRentalApp.Models;
using CarRentalApp.Repository;
using CarRentalApp.Repository.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class EmployeesController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private IEmployees _employees = new Employees();

        public EmployeesController()
        {

        }
        public EmployeesController(ApplicationSignInManager signInManager, ApplicationUserManager userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        [Route("employees")]
        public ActionResult EmployeesIndex()
        {
            return View(_employees.GetAllEmployees().ToList());
        }

        [Route("add-employee")]
        public ActionResult AddEmployees()
        {
            ViewBag.SystemRoles = new SelectList(_employees.GetAllRoles(), "RoleId", "RoleName");
            return View();
        }
        [Route("add-employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> AddEmployees(EmployeeViewModel employeeViewModel)
        {
            ViewBag.SystemRoles = new SelectList(_employees.GetAllRoles(), "RoleId", "RoleName", employeeViewModel.Role);
            try
            {
                Results res = await _employees.AddNewEmployees(employeeViewModel);
                if (res.IsCompletedSuccessFully)
                {
                    TempData["Success"] = "Employee was added successfully.";
                    return RedirectToAction("EmployeesIndex");
                }
                else
                {
                    TempData["Failer"] = res.Errors;
                    return View(employeeViewModel);
                }
            }
            catch
            {
                TempData["Failer"] = "Something went wrong please try again.";
                return View(employeeViewModel);
            }
        }

        [Route("update-employee")]
        public ActionResult UpdateEmployee(string id)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var user = userManager.FindById(id);
                if(user == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                var role = roleManager.FindByName(userManager.GetRoles(user.Id)[0]);
                var employee = new EmployeeViewModel()
                {
                    UserId = id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Contact = user.Contact,
                    Email = user.Email,
                    Role = role.Name
                };
                ViewBag.SystemRoles = new SelectList(_employees.GetAllRoles(), "RoleId", "RoleName",role.Id);
                return View(employee);
            }
        }
        [Route("update-employee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async System.Threading.Tasks.Task<ActionResult> UpdateEmployee(EmployeeViewModel employeeViewModel)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

                ViewBag.SystemRoles = new SelectList(_employees.GetAllRoles(), "RoleId", "RoleName", employeeViewModel.Role);
                try
                {
                    var user = userManager.FindById(employeeViewModel.UserId);
                    user.FirstName = employeeViewModel.FirstName;
                    user.LastName = employeeViewModel.LastName;
                    user.Contact = employeeViewModel.Contact;
                    user.Email = employeeViewModel.Email;
                    var res = userManager.Update(user);

                    if (res.Succeeded)
                    {
                        var remove = userManager.RemoveFromRole(employeeViewModel.UserId, userManager.GetRoles(employeeViewModel.UserId)[0]);
                        if (remove.Succeeded)
                        {
                          await  userManager.AddToRoleAsync(employeeViewModel.UserId, roleManager.FindById(employeeViewModel.Role).Name);
                        }
                        TempData["Success"] = "Employee was updated successfully.";
                        return RedirectToAction("EmployeesIndex");
                    }
                    else
                    {
                        TempData["Failer"] = res.Errors;
                        return View(employeeViewModel);
                    }
                }
                catch
                {
                    TempData["Failer"] = "Something went wrong please try again.";
                    return View(employeeViewModel);
                }
            }
        }

        [HttpGet]
        public JsonResult Delete(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    using(ApplicationDbContext context = new ApplicationDbContext())
                    {
                        var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                        userManager.Delete(userManager.FindById(id));
                    }
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
    }
}