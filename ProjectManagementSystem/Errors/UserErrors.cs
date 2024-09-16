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

    public static readonly Error UserNotVerified =
       new Error("Not Verified.", "user is not verified", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidResetCode=
     new Error("InvalidResetCode", "Invalid reset code", StatusCodes.Status400BadRequest);
}
