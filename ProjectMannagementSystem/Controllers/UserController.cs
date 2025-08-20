using Microsoft.AspNetCore.Mvc;
using ProjectMannagementSystem.Models;

namespace ProjectMannagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly ProjectDbContext _context;
        public UserController(ProjectDbContext context)
        {  _context = context; }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }
        [HttpGet]
        public IActionResult Get()
        {
            var users = _context.Users.ToList();
            return Json(users);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);
            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> Save(User user)
        {
            await _context.Users.AddAsync(user);
            if (await _context.SaveChangesAsync() > 0)
            {
                return Json(new { data = user, msg = "Successfully added" });
            }
            return Json(new { data = user, msg = "Failed to add" });
        }

        [HttpPost]
        public IActionResult Update(User user)
        {
            _context.Users.Update(user);
            if (_context.SaveChanges() > 0)
            {
                return Json(new { data = user, msg = "Successfully updated" });
            }
            return Json(new { data = user, msg = "Failed to update" });
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
