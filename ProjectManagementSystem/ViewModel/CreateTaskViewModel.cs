using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.ViewModel;

public record CreateTaskViewModel(string Title, int ProjectId, int? AssignedToUserId, TaskPriority Priority);

