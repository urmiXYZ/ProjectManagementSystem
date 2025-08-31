
using System.ComponentModel.DataAnnotations;

namespace ProjectMannagementSystem.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }

        [Required, StringLength(150, MinimumLength = 3)]
        public string ProjectName { get; set; }

        [Required, StringLength(1000)]
        public string Description { get; set; }

        public virtual ICollection<AssignedProject> AssignedProjects { get; set; } = new List<AssignedProject>();
    }

}
