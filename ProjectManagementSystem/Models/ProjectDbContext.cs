using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ProjectMannagementSystem.Models
{
    public class ProjectDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
    : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        //  public DbSet<User> Users { get; set; }
        public DbSet<AssignedProject> AssignedProjects { get; set; }
    }
}
