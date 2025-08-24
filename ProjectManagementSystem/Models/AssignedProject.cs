using ProjectMannagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMannagementSystem.Models
{
    public class AssignedProject
    {
        [Key]
        public int AssignedId { get; set; }
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;
        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
    
}
