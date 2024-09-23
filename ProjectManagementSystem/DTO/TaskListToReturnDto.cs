using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.DTO;

public class TaskListToReturnDto
{
    public List<WorkTaskToReturnDto> ToDo { get; set; } = new();
    public List<WorkTaskToReturnDto> InProgress { get; set; } = new();
    public List<WorkTaskToReturnDto> Done { get; set; } = new();
}

public class WorkTaskToReturnDto 
{
    public string Title { get; set; }
    public int? AssignedToUserId { get; set; }

}
