using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Repository;

namespace ProjectManagementSystem.CQRS.Users.Commands
{
    public record ForgotPasswordCommand(string Email) : IRequest<Result<bool>>;
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public ForgotPasswordCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<bool>> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = (await _mediator.Send(new GetUserByEmailQuery(request.Email))).Data;

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
