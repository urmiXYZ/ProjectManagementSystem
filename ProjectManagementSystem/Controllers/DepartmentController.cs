using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class DepartmentController : Controller
    {
        private readonly ProjectDbContext _context;

        public DepartmentController(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var departments = await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.Categories)
                .ToListAsync();
            return View(departments);
        }

        public async Task<IActionResult> CreateOrEdit(int? id)
        {
            if (id == null)
            {
                return PartialView("_CreateOrEditDepartment", new Department());
            }

            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            return PartialView("_CreateOrEditDepartment", dept);
        }

        [HttpPost]
        public IActionResult CreateOrEdit(Department dept)
        {
            if (dept.DepartmentId == 0)
                _context.Departments.Add(dept);
            else
                _context.Departments.Update(dept);
            _context.SaveChanges();
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var dept = _context.Departments.Find(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                _context.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpGet]
        public async Task<IActionResult> GetDepartment(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept == null) return NotFound();

            return Json(new { dept.DepartmentId, dept.Name, dept.Description });
        }

        public JsonResult GetEmployees(int deptId)
        {
            var employees = _context.Users
                .Where(u => u.DepartmentId == deptId)
                .Select(u => new { u.FullName })
                .ToList();
            return Json(employees);
        }

        public JsonResult GetCategories(int deptId)
        {
            var categories = _context.Categories
                .Where(c => c.DepartmentId == deptId)
                .Select(c => new { c.Name })
                .ToList();
            return Json(categories);
        }


    }
}
