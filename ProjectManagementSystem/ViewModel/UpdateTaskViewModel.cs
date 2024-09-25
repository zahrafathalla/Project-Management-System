using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.ViewModel
{
    public class UpdateTaskViewModel
    {
        public string? Title { get; set; }
        public TaskPriority? Priority { get; set; }
    }
}
