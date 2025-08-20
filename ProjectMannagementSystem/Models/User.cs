using System.ComponentModel.DataAnnotations;

namespace ProjectMannagementSystem.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; }
        public byte Age { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime JoinedAt { get; set; }

    }
}
