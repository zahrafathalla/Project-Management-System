using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectManagementSystem.Errors;

public class UserErrors
{
    public static readonly Error InvalidCredentials =
       new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized);
}
