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
        public DbSet<AssignedProject> AssignedProjects { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // 1 Seed Departments
            builder.Entity<Department>().HasData(
                new Department { DepartmentId = 1, Name = "Software Development", Description = "Handles backend and frontend apps" },
                new Department { DepartmentId = 2, Name = "HR", Description = "Handles recruitment and employee management" },
                new Department { DepartmentId = 3, Name = "Marketing", Description = "Promotes products and manages campaigns" }
            );

            // 2 Seed Roles
            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 1, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                new IdentityRole<int> { Id = 2, Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole<int> { Id = 3, Name = "Employee", NormalizedName = "EMPLOYEE" }
            );

            // 3️ Seed Users
            var hasher = new PasswordHasher<User>();

            var superAdmin = new User
            {
                Id = 1,
                UserName = "superadmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "superadmin@pms.com",
                NormalizedEmail = "SUPERADMIN@PMS.COM",
                FullName = "Super Admin User",
                Age = 35,
                PhoneNumber = "01711111111",
                DepartmentId = 1,
                EmailConfirmed = true,
                JoinedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            superAdmin.PasswordHash = hasher.HashPassword(superAdmin, "Super@123");

            var admin = new User
            {
                Id = 2,
                UserName = "admin",
                NormalizedUserName = "ADMIN",
                Email = "admin@pms.com",
                NormalizedEmail = "ADMIN@PMS.COM",
                FullName = "Admin User",
                Age = 30,
                PhoneNumber = "01722222222",
                DepartmentId = 2,
                EmailConfirmed = true,
                JoinedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            admin.PasswordHash = hasher.HashPassword(admin, "Admin@123");

            var employee = new User
            {
                Id = 3,
                UserName = "employee",
                NormalizedUserName = "EMPLOYEE",
                Email = "employee@pms.com",
                NormalizedEmail = "EMPLOYEE@PMS.COM",
                FullName = "Employee User",
                Age = 25,
                PhoneNumber = "01733333333",
                DepartmentId = 3,
                EmailConfirmed = true,
                JoinedAt = DateTime.Now,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            employee.PasswordHash = hasher.HashPassword(employee, "Employee@123");

            builder.Entity<User>().HasData(superAdmin, admin, employee);

            // 4️ Assign Roles to Users
            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { UserId = 1, RoleId = 1 }, // SuperAdmin
                new IdentityUserRole<int> { UserId = 2, RoleId = 2 }, // Admin
                new IdentityUserRole<int> { UserId = 3, RoleId = 3 }  // Employee
            );

            // 5️ Seed Categories
            builder.Entity<Category>().HasData(
                new Category { CategoryId = 1, Name = "Web Applications", Description = "Web-based systems", DepartmentId = 1 },
                new Category { CategoryId = 2, Name = "Recruitment", Description = "Hiring process and tools", DepartmentId = 2 },
                new Category { CategoryId = 3, Name = "Digital Campaigns", Description = "Social media and ad marketing", DepartmentId = 3 }
            );

            // 6️ Seed Projects
            builder.Entity<Project>().HasData(
                new Project { ProjectId = 1, ProjectName = "Task Manager API", Description = "Backend for managing tasks", CategoryId = 1 },
                new Project { ProjectId = 2, ProjectName = "Hiring Portal", Description = "Web system for job postings", CategoryId = 2 },
                new Project { ProjectId = 3, ProjectName = "Social Media Boost", Description = "Automated ad campaign tool", CategoryId = 3 }
            );

            // 7️ Seed AssignedProjects
            builder.Entity<AssignedProject>().HasData(
                new AssignedProject
                {
                    AssignedId = 1,
                    ProjectId = 1,
                    UserId = 3,
                    AssignedDate = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(15),
                    Status = Enums.ProjectStatus.InProgress
                }
            );
        }
    }
}