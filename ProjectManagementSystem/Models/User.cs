using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMannagementSystem.Models
{
    public class User : IdentityUser<int>
    {
        [Required, StringLength(100)]
        public string FullName { get; set; }

        [Range(18, 65, ErrorMessage = "Age must be between 18 and 65.")]
        public byte Age { get; set; }

        [Required, EmailAddress]
        public override string Email { get; set; }

        [Phone]
        public override string PhoneNumber { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.Now;

        public string? PicturePath { get; set; }

        [NotMapped]
        public IFormFile? Picture { get; set; }

        public virtual ICollection<AssignedProject> AssignedProjects { get; set; } = new List<AssignedProject>();
        public int? DepartmentId { get; set; }  

        [ForeignKey("DepartmentId")]
        public virtual Department? Department { get; set; }

    }

}
