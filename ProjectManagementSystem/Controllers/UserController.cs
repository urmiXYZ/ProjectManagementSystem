using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectMannagementSystem.Models;
using System.Text.Json;

namespace ProjectMannagementSystem.Controllers
{

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
        [Authorize(Roles = "Admin,SuperAdmin")]

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        [HttpGet]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Get()
        {
            var users = await _userManager.Users
                .Select(u => new
                {
                    id = u.Id,
                    fullName = u.FullName,
                    userName = u.UserName,
                    email = u.Email,
                    assignedProjects = u.AssignedProjects
                        .Select(ap => new { projectName = ap.Project.ProjectName }).ToList()
                })
                .ToListAsync();

            var result = new List<object>();

            foreach (var u in users)
            {
                var user = await _userManager.FindByIdAsync(u.id.ToString());
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("SuperAdmin"))
                    continue;

                result.Add(new
                {
                    u.id,
                    u.fullName,
                    u.userName,
                    u.email,
                    u.assignedProjects,
                    role = roles.FirstOrDefault() ?? "None"
                });
            }

            return Json(result);
        }


        public IActionResult Profile()
        {
            var userId = int.Parse(_userManager.GetUserId(User));
            ViewBag.UserId = userId;
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetById(int id)
        {
            var user = _context.Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    u.Id,
                    u.FullName,
                    u.UserName,
                    u.Age,
                    u.Email,
                    u.PhoneNumber,
                    u.JoinedAt,
                    u.PicturePath,
                    AssignedProjects = u.AssignedProjects
            .Select(ap => new { ap.Project.ProjectName }).ToList()
                })
                .FirstOrDefault();

            if (user == null) return NotFound();

            if (User.IsInRole("Employee"))
            {
                var loggedInUser = await _userManager.GetUserAsync(User);
                if (loggedInUser == null) return Unauthorized();

                if (user.Id != loggedInUser.Id)
                {
                    return Forbid();
                }
            }

            return Json(user);
        }



        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
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
        [Authorize]

        public async Task<IActionResult> Update([FromBody] User user)
        {
            if (user == null || user.Id <= 0)
                return Json(new { data = user, msg = "User not found" });

            var existingUser = await _userManager.FindByIdAsync(user.Id.ToString());
            if (existingUser == null)
                return Json(new { data = user, msg = "User does not exist" });

            if (User.IsInRole("Employee"))
            {
                var loggedInUserId = _userManager.GetUserId(User);
                if (existingUser.Id.ToString() != loggedInUserId)
                {
                    return Forbid();
                }
            }
            existingUser.FullName = user.FullName;
            existingUser.UserName = user.UserName;
            existingUser.NormalizedUserName = user.UserName.ToUpper();

            existingUser.Email = user.Email;
            existingUser.NormalizedEmail = user.Email.ToUpper();

            existingUser.PhoneNumber = user.PhoneNumber;

            existingUser.Age = user.Age;
            existingUser.JoinedAt = user.JoinedAt;

            var result = await _userManager.UpdateAsync(existingUser);

            if (result.Succeeded)
            {
                return Json(new { data = existingUser, msg = "Successfully updated" });
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { data = user, msg = "Failed to update", errors });
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateProfileWithPicture()
        {
            var form = await Request.ReadFormAsync();
            int id = int.Parse(form["Id"]);
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return Json(new { success = false, msg = "User not found" });

            // Security check
            if (user.Id != int.Parse(_userManager.GetUserId(User)))
                return Forbid();

            // Update profile info
            user.FullName = form["FullName"];
            user.UserName = form["UserName"];
            user.NormalizedUserName = user.UserName.ToUpper();
            user.Email = form["Email"];
            user.NormalizedEmail = user.Email.ToUpper();
            user.PhoneNumber = form["PhoneNumber"];
            user.Age = byte.Parse(form["Age"]);
            user.JoinedAt = DateTime.Parse(form["JoinedAt"]);

            // Handle picture
            var file = form.Files["Picture"];
            if (file != null && file.Length > 0)
            {
                var ext = Path.GetExtension(file.FileName).ToLower();
                if (ext != ".jpg" && ext != ".jpeg" && ext != ".png")
                    return Json(new { success = false, msg = "Invalid file type" });

                var picturesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Pictures");
                if (!Directory.Exists(picturesFolder)) Directory.CreateDirectory(picturesFolder);

                if (!string.IsNullOrEmpty(user.PicturePath))
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.PicturePath.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                var filePath = Path.Combine(picturesFolder, user.Id + ext);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                user.PicturePath = "/Pictures/" + user.Id + ext;
                await _userManager.UpdateAsync(user);

            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Json(new { success = false, msg = errors });
            }

            return Json(new
            {
                success = true,
                fullName = user.FullName,
                userName = user.UserName,
                email = user.Email,
                phoneNumber = user.PhoneNumber,
                age = user.Age,
                joinedAt = user.JoinedAt,
                picturePath = user.PicturePath
            });
        }






        [HttpPost]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return Json(new { msg = "User not found" });
            }

            if (!string.IsNullOrEmpty(user.PicturePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", user.PicturePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.Users.Remove(user);
            if (_context.SaveChanges() > 0)
            {
                return Json(new { msg = "Successfully deleted" });
            }
            return Json(new { msg = "Failed to delete" });
        }

        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> AssignRole([FromBody] JsonElement data)
        {
            try
            {
                var userIds = data.GetProperty("userIds").EnumerateArray()
                                  .Select(x => x.GetInt32()) 
                                  .ToList();

                string role = data.GetProperty("role").GetString();

                foreach (var id in userIds)
                {
                    var user = await _userManager.FindByIdAsync(id.ToString());
                    if (user == null) continue;

                    var roles = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, role);
                }

                return Json(new { msg = "Roles assigned successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { msg = "Server error", error = ex.Message });
            }
        }



    }
}
