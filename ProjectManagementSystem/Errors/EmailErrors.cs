namespace ProjectManagementSystem.Errors
{
    public class EmailErrors
    {
        public static readonly Error EmailSendingFailed =
         new("Email Sending Failed", StatusCodes.Status400BadRequest);

    }
}
