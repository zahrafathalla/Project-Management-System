using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class ResetPasswordCommand : IRequest<Result<bool>>
    {
        public string Email { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ResetPasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = (await _unitOfWork.Repository<User>().GetAsync(u => u.Email == request.Email)).FirstOrDefault();
            if (user == null)
                return Result.Failure<bool>(UserErrors.UserNotFound);

            if (user.PasswordResetCode != request.Code)
                return Result.Failure<bool>(UserErrors.InvalidResetCode);

            user.PasswordHash = PasswordHasher.HashPassword(request.NewPassword); 

            _unitOfWork.Repository<User>().Update(user);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
