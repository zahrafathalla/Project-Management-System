using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModel
{
    public class AssignUsersToProjectViewModel
    {
        [Required]
        public int ProjectId { get; set; }
        [Required]
        public List<int> UserIds { get; set; } = new List<int>();

    }
}
