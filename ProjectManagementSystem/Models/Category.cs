using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMannagementSystem.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<Project> Projects { get; set; } = new List<Project>();
        public int? DepartmentId { get; set; } 
        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }


    }
}
