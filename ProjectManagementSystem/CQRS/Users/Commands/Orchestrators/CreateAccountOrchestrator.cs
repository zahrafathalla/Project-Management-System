using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.CQRS.Users.Commands.Orchestrators
{
    public record CreateAccountOrchestrator(
        string UserName,
        string Email, 
        string Country, 
        string PhoneNumber, 
        string Password) : IRequest<Result<bool>>;


    public class CreateAccountOrchestratorHandler : IRequestHandler<CreateAccountOrchestrator, Result<bool>>
    {
        private readonly IMediator _mediator;

        public CreateAccountOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<bool>> Handle(CreateAccountOrchestrator request, CancellationToken cancellationToken)
        {
            var command = request.Map<CreateAccountCommand>();

            var createAccountResult = await _mediator.Send(command);

            if (!createAccountResult.IsSuccess)
            {
                return Result.Failure<bool>(UserErrors.UserDoesntCreated);
            }

            var userResult = await _mediator.Send(new GetUserByEmailQuery(request.Email));

            if (!userResult.IsSuccess)
            {
                return Result.Failure<bool>(UserErrors.UserDoesntCreated);
            }
            var user = userResult.Data;
            var verificationUrl = $"http://localhost:5097/api/Account/verify?email={user.Email}&token={user.VerificationToken}";

            var emailSent = await EmailSender.SendEmailAsync(
                user.Email,
                "Verify your email",
                $"Please verify your email address by clicking the link: <a href='{verificationUrl}'>Verify your account</a>"
            );

            var IsuserAddedToDefaultRole = await _mediator.Send(new AssignRegisterUserToDefaultRoleCommand(user));

            if (!emailSent)
            {
                Result.Failure<bool>(UserErrors.FailedToSendVerificationEmail);
            }

            return Result.Success(true);
        }
    }
}