namespace ProjectManagementSystem.Errors;

public class UserErrors
{
    public static readonly Error InvalidCredentials =
       new("Invalid email/password", StatusCodes.Status401Unauthorized);

    public static readonly Error InvalidEmail =
       new("Invalid email", StatusCodes.Status404NotFound);

    public static readonly Error UserNotFound =
       new("User Not Found", StatusCodes.Status404NotFound);

    public static readonly Error InvalidCurrentPassword =
        new Error("Current password is incorrect.", StatusCodes.Status400BadRequest);

    public static readonly Error UserNotVerified =
       new Error("user is not verified", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidResetCode =
     new Error("Invalid reset code", StatusCodes.Status400BadRequest);

    public static readonly Error UserAlreadyExists =
    new("User Already Exists", StatusCodes.Status409Conflict);

    public static readonly Error UserDoesntCreated =
    new("User Doesnt Created", StatusCodes.Status409Conflict);

    public static readonly Error FailedToSendVerificationEmail =
    new("Failed to send verification email", StatusCodes.Status409Conflict);

}
