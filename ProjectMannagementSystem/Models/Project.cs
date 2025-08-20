
using System.ComponentModel.DataAnnotations;

namespace ProjectMannagementSystem.Models
{
    public class Project
    {
        [Key]
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }

    }
}
