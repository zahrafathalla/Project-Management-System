using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Errors
{
    public class ProjectErrors
    {
        public static readonly Error ProjectCreationFailed =
          new( "Project Creation Failed", StatusCodes.Status400BadRequest);
        
        public static readonly Error ProjectNotFound =
          new ( "Project Not Found", StatusCodes.Status404NotFound);

        public static readonly Error UserIsNotAssignedToThisProject=
         new( "User is not assigned to this project", StatusCodes.Status404NotFound);

        public static readonly Error FailedToAssignUsersToTheProject =
         new("Failed to assign users to the project", StatusCodes.Status400BadRequest);

    }
}
