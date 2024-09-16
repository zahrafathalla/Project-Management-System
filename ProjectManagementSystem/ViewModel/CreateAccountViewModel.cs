using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.ViewModel;

public class CreateAccountViewModel
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Country { get; set; }
    public string PhoneNumber { get; set; }

    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
     ErrorMessage = "Password must be at least 8 characters long, and include at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
    public string Password { get; set; }
}
