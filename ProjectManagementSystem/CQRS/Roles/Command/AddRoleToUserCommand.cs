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

public record AddRoleToUserCommand (int userId, string roleName) : IRequest<Result<bool>>;

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
        var resultuser = await _mediator.Send(new GetUserByIdQuery(request.userId));
        if (!resultuser.IsSuccess || resultuser.Data == null)
        {
            return Result.Failure<bool>(UserErrors.UserNotFound);
        }

        var roleResult = await _mediator.Send(new GetRoleByNameQuery(request.roleName), cancellationToken);
        if (!roleResult.IsSuccess || roleResult.Data == null)
        {
            return Result.Failure<bool>(RoleErrors.RoleNotFound);
        }
        var role = roleResult.Data;

        var userRolesResult = await _mediator.Send(new GetUserRoleByUserIdQuery(request.userId, role.Id), cancellationToken);

        if (userRolesResult.IsSuccess)
        {
            return Result.Failure<bool>(RoleErrors.RoleAlreadyExists);
        }

        var userRole = new UserRole
        {
            UserId = request.userId,
            RoleId = role.Id
        };

        await _unitOfWork.Repository<UserRole>().AddAsync(userRole);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}