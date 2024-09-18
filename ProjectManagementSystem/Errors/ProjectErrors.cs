namespace ProjectManagementSystem.Errors
{
    public class ProjectErrors
    {
        public static readonly Error ProjectCreationFailed =
          new("Project.ProjectCreationFailed", "Project Creation Failed", StatusCodes.Status400BadRequest);

        public static readonly Error UserAssignmentFailed =
          new("Project.UserAssignmentFailed", "User Assignment Failed", StatusCodes.Status400BadRequest);


    }
}
