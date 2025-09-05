
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
     
        public int? CategoryId { get; set; }

       // [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<AssignedProject> AssignedProjects { get; set; } = new List<AssignedProject>();
    }

}
