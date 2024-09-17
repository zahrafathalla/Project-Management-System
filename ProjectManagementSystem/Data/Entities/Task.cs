using ProjectManagementSystem.Enum;

namespace ProjectManagementSystem.Data.Entities;

public class TasK
{
    public int Id { get; set; }
    public string Title { get; set; }
    public TasKStatus Status { get; set; } = TasKStatus.ToDo;
    public DateTime DateCreated { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }

    public int? AssignedToUserId { get; set; }
    public User? AssignedToUser { get; set; }
}
