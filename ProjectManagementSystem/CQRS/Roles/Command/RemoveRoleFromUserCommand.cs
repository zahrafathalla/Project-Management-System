using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Roles.Query;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Roles.Command
{
    public record RemoveRoleFromUserCommand(int UserId, int RoleId) : IRequest<Result<bool>>;

    public class RemoveRoleFromUserHandler : IRequestHandler<RemoveRoleFromUserCommand, Result<bool>>
    {
        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public RemoveRoleFromUserHandler(IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
        {
            var roleResult = await _mediator.Send(new GetRoleByIdQuery(request.RoleId));
            if (!roleResult.IsSuccess)
            {
                return Result.Failure<bool>(RoleErrors.RoleNotFound);
            }
            var role = roleResult.Data;

            var userResult = await _mediator.Send(new GetUserByIdQuery(request.UserId));
            if (!userResult.IsSuccess )
            {
                return Result.Failure<bool>(UserErrors.UserNotFound);
            }
            var user = userResult.Data;


            var userRole = (await _unitOfWork.Repository<UserRole>()
                .GetAsync(ur => ur.UserId == request.UserId && ur.RoleId == request.RoleId)).FirstOrDefault();

            if (userRole == null)
            {
                return Result.Failure<bool>(RoleErrors.RoleNotAssigned);
            }

            _unitOfWork.Repository<UserRole>().Delete(userRole);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
