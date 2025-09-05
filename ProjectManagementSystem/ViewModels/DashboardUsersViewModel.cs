using ProjectMannagementSystem.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class DashboardUsersViewModel
    {
        public IEnumerable<User> Users { get; set; } = new List<User>();

        public User NewUser { get; set; } = new User();
    }

}
