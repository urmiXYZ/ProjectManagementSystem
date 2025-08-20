using Microsoft.AspNetCore.Mvc;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    public class ProjectController : Controller
    {
        private readonly ProjectDbContext _dbContext;

        public ProjectController(ProjectDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var tasks = _dbContext.Projects.ToList();
            return View(tasks);
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
        public IActionResult Get()
        {
            var projects = _dbContext.Projects.ToList();
            return Json(projects);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var project = _dbContext.Projects.Find(id);
            return Json(project);
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
    }
}
