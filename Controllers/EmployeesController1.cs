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
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(EmpVM vm)
        {
            if (ModelState.IsValid)
            {
                Employee employee = new Employee
                {
                    EmployeeName = vm.EmployeeName,
                    BirthDate = vm.BirthDate,
                    DesignationId = vm.DesignationId
                };

                if (vm.PictureFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.PictureFile.FileName);
                    string filepath = Path.Combine(_env.WebRootPath, "Images", fileName);
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await vm.PictureFile.CopyToAsync(stream);
                    }
                    employee.Picture = fileName;
                }

                _emp.Employees.Add(employee);
                await _emp.SaveChangesAsync();
                return Json(new { success = true, message = "Saved Successfully!" });
            }

            return Json(new { success = false, message = "Validation Failed!" });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _emp.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            var vm = new EmpVM
            {
                EmployeeId = employee.EmployeeId,
                EmployeeName = employee.EmployeeName,
                BirthDate = employee.BirthDate,
                DesignationId = employee.DesignationId,
                Picture = employee.Picture
            };

            ViewBag.designation = _emp.Designations.ToList();
            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmpVM vm)
        {
            if (ModelState.IsValid)
            {
                var employee = await _emp.Employees.FindAsync(vm.EmployeeId);
                if (employee == null) return NotFound();

                employee.EmployeeName = vm.EmployeeName;
                employee.BirthDate = vm.BirthDate;
                employee.DesignationId = vm.DesignationId;

                if (vm.PictureFile != null)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.PictureFile.FileName);
                    string filepath = Path.Combine(_env.WebRootPath, "Images", fileName);
                    using (var stream = new FileStream(filepath, FileMode.Create))
                    {
                        await vm.PictureFile.CopyToAsync(stream);
                    }
                    employee.Picture = fileName;
                }

                await _emp.SaveChangesAsync();
                return Ok(); 
            }
            return BadRequest();
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var employee = await _emp.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            _emp.Employees.Remove(employee);
            await _emp.SaveChangesAsync();
            return Json(new { success = true }); 
        }
    }
}
