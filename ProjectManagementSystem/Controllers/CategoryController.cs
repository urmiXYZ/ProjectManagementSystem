using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]

    public class CategoryController : Controller
    {
        private readonly ProjectDbContext _context;

        public CategoryController(ProjectDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories
                                           .Include(c => c.Projects)
                                           .Include(c => c.Department)
                                           .ToListAsync();

            ViewBag.Departments = await _context.Departments.ToListAsync();

            return View(categories);
        }


        [HttpPost]
        public async Task<IActionResult> Save(Category category)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage);
                return Json(new { success = false, errors });
            }

            if (category.CategoryId == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                _context.Categories.Update(category);
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var category = await _context.Categories
                                         .Include(c => c.Department)
                                         .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();
            return Json(category);
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories
                                         .Include(c => c.Projects) 
                                         .FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category == null) return NotFound();

            _context.Categories.Remove(category); 
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
