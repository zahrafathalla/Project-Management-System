using MediatR;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.CQRS.Users.Commands.Orchestrators
{
    public class CreateAccountOrchestrator : IRequest<CreateAccountOrchestratorToReturnDto>
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class CreateAccountOrchestratorToReturnDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
    }

    public class CreateAccountOrchestratorHandler : IRequestHandler<CreateAccountOrchestrator, CreateAccountOrchestratorToReturnDto>
    {
        private readonly IMediator _mediator;

        public CreateAccountOrchestratorHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateAccountOrchestratorToReturnDto> Handle(CreateAccountOrchestrator request, CancellationToken cancellationToken)
        {
            var createAccountResult = await _mediator.Send(new CreateAccountCommand
            {
                UserName = request.UserName,
                Email = request.Email,
                Country = request.Country,
                PhoneNumber = request.PhoneNumber,
                Password = request.Password
            });

            if (!createAccountResult.IsSuccessed)
            {
                return new CreateAccountOrchestratorToReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = createAccountResult.ErrorMessage
                };
            }

            var user = await _mediator.Send(new GetUserByEmailQuery(request.Email));

            if (user == null)
            {
                return new CreateAccountOrchestratorToReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found "
                };
            }

            var verificationUrl = $"http://localhost:5097/api/Account/verify?email={user.Data.Email}&token={user.Data.VerificationToken}";


            var emailSent = await _mediator.Send(new SendVerificationEmailCommand
            {
                ToEmail = user.Data.Email,
                Subject = "Verify your email",
                Body = $"Please verify your email address by clicking the link: <a href='{verificationUrl}'>Verify your account</a>"
            });


           var IsAddedToDefaultRole =  await _mediator.Send(new AssignRegisterUserToDefaultRoleCommand(user.Data));

            if (!emailSent)
            {
                return new CreateAccountOrchestratorToReturnDto
                {
                    IsSuccess = false,
                    ErrorMessage = "Failed to send verification email."
                };
            }

            return new CreateAccountOrchestratorToReturnDto
            {
                Id = createAccountResult.Id,
                Email = createAccountResult.Email
            };
        }
    }
}
