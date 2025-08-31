using ProjectManagementSystem.Models;
using ProjectMannagementSystem.Models;

public class DashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalProjects { get; set; }
    public int ActiveProjects { get; set; }

    public int SuperAdminCount { get; set; }
    public int AdminCount { get; set; }
    public int EmployeeCount { get; set; }

    public List<User> RecentUsers { get; set; }
    public List<Project> RecentProjects { get; set; }

    // Employee-specific
    public int MyProjectsCount { get; set; }
    public List<ProjectEvent> MyProjectEvents { get; set; } = new List<ProjectEvent>();
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int OnHoldCount { get; set; }
    public int CancelledCount { get; set; }


}
