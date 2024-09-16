using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Roles.Query;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Context;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using System.Linq;

namespace ProjectManagementSystem.CQRS.Roles.Command;

public class AddRoleToUserCommand : IRequest<Result<bool>>
{

    public User user { get; set; }
    public string roleName { get; set; }

    public AddRoleToUserCommand(User user,string roleName)
    {
        this.roleName = roleName;
        this.user = user;
    }
}

public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserCommand, Result<bool>>
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public AddRoleToUserHandler(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var roleResult = await _mediator.Send(new GetRoleByNameQuery(request.roleName), cancellationToken);
        if (!roleResult.IsSuccess || roleResult.Data == null)
        {
            return Result.Failure<bool>(RoleErrors.RoleNotFound);
        }
        var role = roleResult.Data;

        var userRolesResult = await _mediator.Send(new GetUserRoleByUserIdQuery(request.user.Id, role.Id), cancellationToken);
       
        if (userRolesResult.Data.Any(r => r.RoleId == role.Id))
        {
            return Result.Failure<bool>(RoleErrors.RoleAlreadyExists);
        }

        var userRole = new UserRole
        {
            UserId = request.user.Id,
            RoleId = role.Id
        };

        await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}
