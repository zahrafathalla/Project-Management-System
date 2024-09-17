using ProjectManagementSystem.Enum;

namespace ProjectManagementSystem.Data.Entities;

public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Public;
    public DateTime DateCreated { get; set; }

    public int CreatedByUserId { get; set; }
    public User CreatedByUser { get; set; }

    public ICollection<TasK> Tasks { get; set; } = new HashSet<TasK>();
    public ICollection<UserProject> UserProjects { get; set; } = new HashSet<UserProject>();
}
