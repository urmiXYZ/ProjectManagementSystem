using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMannagementSystem.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required, StringLength(300)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public virtual ICollection<User> Employees { get; set; } = new List<User>();

        public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
