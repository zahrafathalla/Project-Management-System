using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Context;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public class AddRoleToUserCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public string RoleName { get; set; }
}

public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserCommand, bool>
{
    private readonly ApplicationDBContext _dbContext;

    public AddRoleToUserHandler(ApplicationDBContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _dbContext.users.FindAsync(request.UserId);
        if (user == null)
        {
            return false; // User not found
        }

        var role = await _dbContext.roles.SingleOrDefaultAsync(r => r.Name == request.RoleName);
        if (role == null)
        {
            return false; // Role not found
        }

        var existingUserRole = await _dbContext.UserRoles
            .AnyAsync(ur => ur.UserId == request.UserId && ur.RoleId == role.Id);

        if (existingUserRole)
        {
            return true; // Role already assigned
        }

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        await _dbContext.UserRoles.AddAsync(userRole);
        await _dbContext.SaveChangesAsync();

        return true; // Successfully added the role
    }
}


