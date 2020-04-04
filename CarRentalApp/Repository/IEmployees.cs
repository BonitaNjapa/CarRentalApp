using CarRentalApp.Models;
using CarRentalApp.Repository.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApp.Repository
{
    public interface IEmployees
    {
        ICollection<EmployeeViewModel> GetAllEmployees();
        Task<Results> AddNewEmployees(EmployeeViewModel employeeViewModel);
        ICollection<RolesViewModel> GetAllRoles();
        int EmpCount();
    }
}
