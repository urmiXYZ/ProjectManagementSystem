using ProjectManagementSystem.Validators;
using ProjectMannagementSystem.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMannagementSystem.Models
{
    public class AssignedProject
    {
        [Key]
        public int AssignedId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        [FutureDateAttribute]
        public DateTime DueDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime? SubmitDate { get; set; }

        [Required]
        public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }


}
