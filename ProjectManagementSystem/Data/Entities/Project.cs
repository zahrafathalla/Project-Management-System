using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.Data.Entities;

public class Project : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Public;
    public DateTime DateCreated { get; set; }
    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }

    public ICollection<WorkTask> Tasks { get; set; } = new HashSet<WorkTask>();
    public ICollection<UserProject> UserProjects { get; set; } = new HashSet<UserProject>();
}
