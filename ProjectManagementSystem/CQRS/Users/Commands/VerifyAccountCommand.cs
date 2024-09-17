using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public record VerifyAccountCommand(string Email, string Token) : IRequest<Result<bool>>;

    public class VerifyAccountCommandHandler : IRequestHandler<VerifyAccountCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public VerifyAccountCommandHandler(
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(VerifyAccountCommand request, CancellationToken cancellationToken)
        {
            var userResult = await _mediator.Send(new GetUserForVerificationQuery(request.Email, request.Token));

            if (!userResult.IsSuccess)
            {
                return Result.Failure<bool>(UserErrors.UserNotFound);
            }

            var user = userResult.Data;

            user.IsEmailVerified = true;
            user.VerificationToken = null;

            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
