using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Entities;

public class WorkTask : BaseEntity
{
    public string Title { get; set; }
    public WorkTaskStatus Status { get; set; } = WorkTaskStatus.ToDo;
    public DateTime DateCreated { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public int? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }

}
