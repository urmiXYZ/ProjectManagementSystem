using Microsoft.EntityFrameworkCore;

namespace ProjectMannagementSystem.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> Options) : base(Options)
        { }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AssignedProject> AssignedProjects { get; set; }
    }
}
