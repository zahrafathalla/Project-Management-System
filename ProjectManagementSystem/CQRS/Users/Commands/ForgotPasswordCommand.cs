using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Repository;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public class ForgotPasswordCommand : IRequest<Result<bool>>
    {
        public string Email { get; set; }
    }
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            //temporarity untill i change GetUserByEmailQuery to return user not result(user)
            var userResult = await _unitOfWork.Repository<User>().GetAsync(u => u.Email == request.Email);
            var user = userResult.FirstOrDefault();
            
            if (user == null)
                return Result.Failure<bool>(UserErrors.UserNotFound);

            if (!user.IsEmailVerified)
                return Result.Failure<bool>(UserErrors.UserNotVerified);


            var resetCode = Guid.NewGuid().ToString();
            user.PasswordResetCode = resetCode;
            await _unitOfWork.SaveChangesAsync();

            var resetUrl = $"https://localhost:7120/api/Account/reset-password?email={request.Email}&code={resetCode}";

            var emailSent = await EmailSender.SendEmailAsync(request.Email, "Reset Your Password", $"Please reset your password by clicking the link: <a href='{resetUrl}'>Reset Password</a>");

            return Result.Success(emailSent);
        }
    }
}
