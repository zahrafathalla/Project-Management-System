using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectManagementSystem.Errors;

public class UserErrors
{
    public static readonly Error InvalidCredentials =
       new("User.InvalidCredentials", "Invalid email/password", StatusCodes.Status401Unauthorized); 
    
    public static readonly Error InvalidEmail =
       new("InvalidEmail", "Invalid email/password", StatusCodes.Status404NotFound);
    
    public static readonly Error UserNotFound =
       new("UserNotFound", "User Not Found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidCurrentPassword = 
        new Error("Current password is incorrect.", "Current password is incorrect.",StatusCodes.Status400BadRequest);

}
