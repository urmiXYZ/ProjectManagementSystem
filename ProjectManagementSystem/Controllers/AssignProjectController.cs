using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProjectMannagementSystem.Enums;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    public class AssignProjectController : Controller
    {
        private readonly ProjectDbContext _context;
        private readonly UserManager<User> _userManager;
        public AssignProjectController(UserManager<User> userManager, ProjectDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var roles = await _userManager.GetRolesAsync(currentUser);

            var query = _context.AssignedProjects
                .Include(ap => ap.Project)
                .Include(ap => ap.User)
                .AsQueryable();

            if (roles.Contains("Employee"))
            {
                query = query.Where(ap => ap.UserId == currentUser.Id);
            }

            var assignedProjects = await query
                .Select(ap => new
                {
                    ap.AssignedId,
                    Project = new { ap.Project.ProjectName },
                    User = new { ap.User.UserName, ap.User.Email },
                    ap.AssignedDate,
                    ap.DueDate,
                    ap.SubmitDate,
                    Status = ap.Status.ToString()
                })
                .ToListAsync();

            return Json(assignedProjects);
        }

        [HttpPost]
        public IActionResult Assign([FromBody] List<AssignedProject> models)
        {
            foreach (var model in models)
            {
                model.AssignedDate = DateTime.Now;
                if (model.Status == 0)
                {
                    model.Status = ProjectStatus.InProgress;
                }
                _context.AssignedProjects.Add(model);
            }

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public IActionResult ChangeDueDateStatus([FromBody] AssignedProject model)
        {
            if (model == null) return BadRequest("Invalid data");

            var assignment = new AssignedProject { AssignedId = model.AssignedId };
            _context.AssignedProjects.Attach(assignment);

            assignment.DueDate = model.DueDate;

            assignment.Status = model.Status;

            _context.Entry(assignment).Property(ap => ap.DueDate).IsModified = true;
            _context.Entry(assignment).Property(ap => ap.Status).IsModified = true;

            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Employee")]
        public IActionResult UpdateSubmitDate([FromBody] AssignedProject model)
        {
            if (model == null) return BadRequest("Invalid data");

            var assignment = _context.AssignedProjects
                .FirstOrDefault(ap => ap.AssignedId == model.AssignedId);

            if (assignment == null) return NotFound();

            var currentUserId = int.Parse(_userManager.GetUserId(User));
            if (assignment.UserId != currentUserId)
                return Forbid();

            assignment.SubmitDate = model.SubmitDate ?? DateTime.Now;

            _context.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IActionResult SubmitTask([FromBody] dynamic data)
        {
            int assignedId = data.assignedId;
            var assignment = _context.AssignedProjects.Find(assignedId);

            if (assignment == null)
                return NotFound(new { msg = "Task not found" });

            assignment.SubmitDate = DateTime.Now;
            _context.SaveChanges();

            return Ok(new { msg = "Task submitted successfully" });
        }





    }
}