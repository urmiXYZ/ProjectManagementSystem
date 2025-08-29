using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectMannagementSystem.Models
{
    public class User : IdentityUser<int>
    {
        //[Key]
        // public int UserId { get; set; }
        // public string UserName { get; set; }
        public string FullName { get; set; }
        public byte Age { get; set; }
     //   public string Email { get; set; }
      //  public string Phone { get; set; }
        public DateTime JoinedAt { get; set; }
        public string? PicturePath { get; set; }
        [NotMapped]
        public IFormFile? Picture { get; set; }
        public virtual ICollection<AssignedProject> AssignedProjects { get; set; } = new List<AssignedProject>();


    }
}
