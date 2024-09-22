namespace ProjectManagementSystem.Errors;

public class TaskErrors
{
    public static readonly Error TaskNotFound =
        new("Task is not found", StatusCodes.Status404NotFound);

    public static readonly Error UserIsAlreadyAssignedToThisTask =
        new("User is not assigned to this Task", StatusCodes.Status409Conflict);

    public static readonly Error TaskAlreadyAssigned =
       new("Task Is already assigned to user", StatusCodes.Status409Conflict);

}
