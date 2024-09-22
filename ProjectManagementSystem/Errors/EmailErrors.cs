namespace ProjectManagementSystem.Errors
{
    public class EmailErrors
    {
        public static readonly Error EmailSendingFailed =
         new("Email Sending Failed", StatusCodes.Status400BadRequest);

        public static readonly Error InvitationAlreadyAccepted =
         new("Invitation has already been accepted", StatusCodes.Status400BadRequest);

        public static readonly Error InvitationAlreadyRejected =
          new("Invitation has already been rejected", StatusCodes.Status400BadRequest);

    }
}
