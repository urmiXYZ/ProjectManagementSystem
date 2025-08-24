using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    [Authorize]
    public class UserController : Controller

    {
        private readonly UserManager<User> _userManager;
        private readonly ProjectDbContext _context;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public UserController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager, ProjectDbContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users
                .Select(u => new {
                    u.Id,
                    u.UserName,
                    u.Email,
                    AssignedProjects = u.AssignedProjects
                        .Select(ap => new { ap.Project.ProjectName }).ToList()
                }).ToList();

            return Json(users);
        }
        public IActionResult Profile()
        {
            var userId = int.Parse(_userManager.GetUserId(User));
            ViewBag.UserId = userId;
            return View();
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var user = _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Age,
                    u.Email,
                    u.PhoneNumber,
                    u.JoinedAt
                })
                .FirstOrDefault();

            if (user == null) return NotFound();
            return Json(user);
        }



        [HttpPost]
        public async Task<IActionResult> Save(User user)
        {
            string password = $"{user.UserName.ToUpper().Substring(0, 3)}*566#p";

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                if (!await _roleManager.RoleExistsAsync("Employee"))
                    await _roleManager.CreateAsync(new IdentityRole<int>("Employee"));

                await _userManager.AddToRoleAsync(user, "Employee");

                return Json(new { data = user, password, msg = "Successfully added" });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { data = user, msg = "Failed to add", errors });
        }



        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null || user.Id <= 0)
                return Json(new { data = user, msg = "User not found" });

            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existingUser == null)
                return Json(new { data = user, msg = "User does not exist" });

            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Age = user.Age;
            existingUser.JoinedAt = user.JoinedAt;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
                return Json(new { data = existingUser, msg = "Successfully updated" });

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { data = user, msg = "Failed to update", errors });
        }




        [HttpPost]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return Json(new { msg = "User not found" });
            }

            _context.Users.Remove(user);
            if (_context.SaveChanges() > 0)
            {
                return Json(new { msg = "Successfully deleted" });
            }
            return Json(new { msg = "Failed to delete" });
        }




    }
}
