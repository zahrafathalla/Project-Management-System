using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Context;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public class AddRoleToUserCommand : IRequest<bool>
{
    public int UserId { get; set; }
    public string RoleName { get; set; }
}

public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;

    public AddRoleToUserHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.Repository<User>().GetAsync(r=>r.Id == request.UserId);
        var user = users.FirstOrDefault();

        if (user == null)
        {
            return false; // User not found
        }

        var roles = await _unitOfWork.Repository<Role>().GetAsync(r => r.Name == request.RoleName);
        var role = roles.FirstOrDefault();

        if (role == null)
        {
            return false; // Role not found
        }

        var existingUserRoles = await _unitOfWork.Repository<UserRole>()
            .GetAsync(ur => ur.UserId == request.UserId && ur.RoleId == role.Id);


        if (existingUserRoles.FirstOrDefault() == null)
        {
            return true; // Role already assigned
        }

        var userRole = new UserRole
        {
            UserId = user.Id,
            RoleId = role.Id
        };

        await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
        await _unitOfWork.SaveChangesAsync();

        return true; // Successfully added the role
    }
}


