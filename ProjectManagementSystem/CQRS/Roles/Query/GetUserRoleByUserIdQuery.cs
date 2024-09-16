using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using System.Data;

namespace ProjectManagementSystem.CQRS.Roles.Query;

public record GetUserRoleByUserIdQuery(int UserId, int RoleId) : IRequest<Result<IEnumerable<UserRole>>>;

public class GetUserRoleByUserIdQueryHandler : IRequestHandler<GetUserRoleByUserIdQuery, Result<IEnumerable<UserRole>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetUserRoleByUserIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<IEnumerable<UserRole>>> Handle(GetUserRoleByUserIdQuery request, CancellationToken cancellationToken)
    {
        var UserRoles = await _unitOfWork.Repository<UserRole>()
           .GetAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId);

        return Result.Success(UserRoles);
    }

}
