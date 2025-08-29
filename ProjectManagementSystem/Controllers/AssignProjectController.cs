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
        [Authorize]


        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]

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
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        public IActionResult SubmitTask([FromForm] int assignId)
        {
            var assignment = _context.AssignedProjects.Find(assignId);

            if (assignment == null)
                return NotFound(new { msg = "Task not found" });
            if (assignment.Status == ProjectStatus.OnHold ||
        assignment.Status == ProjectStatus.Cancelled)
            {
                return BadRequest(new { msg = $"Cannot submit task. Status is {assignment.Status}." });
            }
            assignment.SubmitDate = DateTime.Now;
            assignment.Status = ProjectStatus.Completed;

            _context.SaveChanges();

            return Ok(new { msg = "Task submitted successfully" });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult ResubmitTask([FromForm] int assignId, [FromForm] DateTime dueDate)
        {
            var assignment = _context.AssignedProjects.Find(assignId);
            if (assignment == null)
                return NotFound(new { msg = "Task not found" });

            if (assignment.Status != ProjectStatus.Completed)
                return BadRequest(new { msg = "Only completed tasks can be resubmitted" });

            assignment.DueDate = dueDate;
            assignment.Status = ProjectStatus.InProgress;
            assignment.SubmitDate = null;

            _context.SaveChanges();
            return Ok(new { msg = "Task resubmitted successfully" });
        }




    }
}