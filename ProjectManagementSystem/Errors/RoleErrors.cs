﻿namespace ProjectManagementSystem.Errors;

public class RoleErrors
{
    public static readonly Error RoleNotFound =
        new("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);


    public static readonly Error DuplicatedRole =
        new("Role.DuplicatedRole", "Another role with the same name is already exists", StatusCodes.Status409Conflict);

    public static readonly Error RoleAlreadyExists =
        new("Role.RoleAlreadyExists", "Role Already Exists", StatusCodes.Status409Conflict);

    public static readonly Error RoleNotAssigned =
       new("Role.RoleNotAssigned", "Role Not Assigned", StatusCodes.Status400BadRequest);
}
