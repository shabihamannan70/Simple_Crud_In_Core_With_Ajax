using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using work_01.Models;

namespace work_01.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeDbContext _emp;
        private readonly IWebHostEnvironment _env;
        public EmployeesController(EmployeeDbContext emp, IWebHostEnvironment env)
        {
            _emp = emp;
            _env = env;
        }
        public IActionResult Index()
        {
            var employee= _emp.Employees.Include(x=> x.Designation).ToList();
            return View(employee);
        }
        public IActionResult Create()
        {
            ViewBag.designation = _emp.Designations.ToList();
            return View();
        }
    }
}
