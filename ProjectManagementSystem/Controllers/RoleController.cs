using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectMannagementSystem.Models;


namespace ProjectMannagementSystem.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleController(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return Json(new { success = false, msg = "Role name required" });

            if (await _roleManager.RoleExistsAsync(roleName))
                return Json(new { success = false, msg = "Role already exists" });

            var result = await _roleManager.CreateAsync(new IdentityRole<int>(roleName));
            if (result.Succeeded)
                return Json(new { success = true, msg = "Role created successfully", roleName });

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, msg = errors });
        }
    }
}