using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Roles.Query
{
    public record GetRoleByIdQuery(int RoleId) : IRequest<Result<Role>>;

    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, Result<Role>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetRoleByIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Role>> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _unitOfWork.Repository<Role>().GetByIdAsync(request.RoleId);
            if (role == null)
            {
                return Result.Failure<Role>(RoleErrors.RoleNotFound);
            }

            return Result.Success(role);
        }
    }
}
