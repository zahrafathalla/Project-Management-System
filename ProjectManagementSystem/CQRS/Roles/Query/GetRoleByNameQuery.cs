using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Roles.Query;

public record GetRoleByNameQuery(string Name) : IRequest<Result<Role>>;

public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, Result<Role>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetRoleByNameQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Role>> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        var role = (await _unitOfWork.Repository<Role>().GetAsync(u => u.Name == request.Name)).FirstOrDefault();
        if (role == null)
        {
            return Result.Failure<Role>(RoleErrors.RoleNotFound);
        }

        return Result.Success(role);
    }
}