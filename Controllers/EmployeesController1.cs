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

                // সফল হলে JSON রিটার্ন করুন
                return Json(new { success = true, message = "Saved Successfully!" });
            }

            return Json(new { success = false, message = "Validation Failed!" });
        }


        // ১. এডিট পেজ লোড করা (GET)
        public async Task<IActionResult> Edit(int id)
        {
            var employee = await _emp.Employees.FindAsync(id);
            if (employee == null) return NotFound();

            // ম্যাপ করুন Employee থেকে EmpVM-এ
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

        // ২. ডাটা আপডেট করা (POST)
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
                    employee.Picture = fileName; // নতুন ছবি সেভ
                }

                await _emp.SaveChangesAsync();
                return Ok(); // Ajax সফল হওয়ার জন্য
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
            return Json(new { success = true }); // Ajax-এর জন্য JSON রিটার্ন করা ভালো
        }
    }
}
