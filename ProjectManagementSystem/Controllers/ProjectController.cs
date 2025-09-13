using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class ProjectController : Controller
    {
        private readonly ProjectDbContext _dbContext;

        public ProjectController(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            ViewBag.Categories = _dbContext.Categories.ToList();
            var tasks = _dbContext.Projects.ToList();
            return View(tasks);
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            var projects = _dbContext.Projects
                             .Include(p => p.Category)
                             .ThenInclude(c => c.Department)
                             .ToList();

            var result = projects.Select(p => new {
                ProjectId = p.ProjectId,
                ProjectName = p.ProjectName,
                Description = p.Description,
                CategoryName = p.Category != null ? p.Category.Name : "None",
                DepartmentName = p.Category != null && p.Category.Department != null
                                    ? p.Category.Department.Name
                                    : "None"
            }).ToList();

            return Json(result);
        }



        [HttpGet]
        public IActionResult GetAllByDepartment(int departmentId)
        {
            var projects = _dbContext.Projects
                .Include(p => p.Category)
                .Where(p => p.Category.DepartmentId == departmentId) // <- department via category
                .Select(p => new {
                    ProjectId = p.ProjectId,
                    ProjectName = p.ProjectName,
                    CategoryName = p.Category != null ? p.Category.Name : "None"
                })
                .ToList();

            return Json(projects);
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Project project)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Projects.Add(project);
                if (_dbContext.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError("", "Failed to create Project. Please try again.");
            }
            return View(project);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var project = _dbContext.Projects
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProjectId == id);

            if (project == null) return NotFound();

            return Json(new
            {
                project.ProjectId,
                project.ProjectName,
                project.Description,
                CategoryId = project.CategoryId
            });
        }


        [HttpPost]
        public async Task<IActionResult> Save(Project project)
        {
            await _dbContext.Projects.AddAsync(project);
            if (await _dbContext.SaveChangesAsync() > 0)
            {
                return Json(new { data = project, msg = "Successfully added" });
            }
            return Json(new { data = project, msg = "Failed to add" });
        }

        [HttpPost]
        public IActionResult Update(Project project)
        {
            _dbContext.Projects.Update(project);
            if (_dbContext.SaveChanges() > 0)
            {
                return Json(new { data = project, msg = "Successfully updated" });
            }
            return Json(new { data = project, msg = "Failed to update" });
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var project = _dbContext.Projects.Find(id);
            if (project == null)
            {
                return Json(new { msg = "Project not found" });
            }

            _dbContext.Projects.Remove(project);
            if (_dbContext.SaveChanges() > 0)
            {
                return Json(new { msg = "Successfully deleted" });
            }
            return Json(new { msg = "Failed to delete" });
        }


        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _dbContext.Categories
                .Select(c => new { c.CategoryId, c.Name })
                .ToList();
            return Json(categories);
        }

    }
}
