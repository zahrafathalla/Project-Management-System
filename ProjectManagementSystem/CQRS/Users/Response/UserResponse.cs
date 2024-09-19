namespace ProjectManagementSystem.CQRS.Users.Response;

public class UserResponse
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Status { get; set; }
}
