using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public record GetUserForVerificationQuery(string Email, string Token) : IRequest<Result<User>>;
    public class GetUserForVerificationQueryHandler : IRequestHandler<GetUserForVerificationQuery, Result<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserForVerificationQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> Handle(GetUserForVerificationQuery request, CancellationToken cancellationToken)
        {
            var user = (await _unitOfWork.Repository<User>()
                               .GetAsync(u => u.Email == request.Email && u.VerificationToken == request.Token && !u.IsEmailVerified))
                               .FirstOrDefault();

            if (user == null)
            {
                return Result.Failure<User>(UserErrors.UserNotFound);

            }
            return Result.Success(user);

        }
    }
}
