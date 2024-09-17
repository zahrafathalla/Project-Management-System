namespace ProjectManagementSystem.Data.Entities;

public class UserProject
{
    public int Id { get; set; }
    public bool IsCreator { get; set; } = false; 

    public int UserId { get; set; }
    public User User { get; set; }

    public int ProjectId { get; set; }
    public Project Project { get; set; }
}
