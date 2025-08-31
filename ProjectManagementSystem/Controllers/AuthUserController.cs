using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;
using ProjectMannagementSystem.Enums;
using ProjectMannagementSystem.Models;

public class AuthUserController : Controller
{
    private readonly ProjectDbContext _context;
    private readonly UserManager<User> _userManager;

    public AuthUserController(ProjectDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Dashboard()
    {
        var currentUser = await _userManager.GetUserAsync(User);
        var assignedProjects = await _context.AssignedProjects
        .Include(ap => ap.Project)
        .Where(ap => ap.UserId == currentUser.Id)
        .ToListAsync();
        var model = new DashboardViewModel
        {
            TotalUsers = await _context.Users.CountAsync(),
            TotalProjects = await _context.Projects.CountAsync(),
            ActiveProjects = await _context.AssignedProjects.CountAsync(p => p.Status == ProjectStatus.InProgress),

            SuperAdminCount = (await _userManager.GetUsersInRoleAsync("SuperAdmin")).Count,
            AdminCount = (await _userManager.GetUsersInRoleAsync("Admin")).Count,
            EmployeeCount = (await _userManager.GetUsersInRoleAsync("Employee")).Count,

            RecentUsers = await _context.Users
                .OrderByDescending(u => u.JoinedAt)
                .Take(5)
                .ToListAsync(),

            RecentProjects = await _context.Projects
                .OrderByDescending(p => p.ProjectId)
                .Take(5)
                .ToListAsync(),

            MyProjectsCount = await _context.AssignedProjects.CountAsync(ap => ap.UserId == currentUser.Id),
            InProgressCount = assignedProjects.Count(ap => ap.Status == ProjectStatus.InProgress),
            CompletedCount = assignedProjects.Count(ap => ap.Status == ProjectStatus.Completed),
            OnHoldCount = assignedProjects.Count(ap => ap.Status == ProjectStatus.OnHold),
            CancelledCount = assignedProjects.Count(ap => ap.Status == ProjectStatus.Cancelled),

            MyProjectEvents = assignedProjects.Select(ap => new ProjectEvent
            {
                Title = ap.Project.ProjectName,
                Start = ap.DueDate,
                End = ap.SubmitDate,
                Color = ap.Status switch
                {
                    ProjectStatus.InProgress => "#f39c12", // yellow
                    ProjectStatus.Completed => "#00a65a",  // green
                    ProjectStatus.OnHold => "#f56954",     // red
                    ProjectStatus.Cancelled => "#f56954",  // red
                    _ => "#3c8dbc"                         // default blue
                }
            }).ToList()

        };

        return View(model);
    }

}
