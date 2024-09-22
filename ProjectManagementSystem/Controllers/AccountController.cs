using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Commands.Orchestrators;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.ViewModel;

namespace ProjectManagementSystem.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("CreateAccount")]
        public async Task<Result<bool>> CreateAccount([FromBody] CreateAccountViewModel viewModel)
        {
            var command = viewModel.Map<CreateAccountOrchestrator>();

            var response = await _mediator.Send(command);
            return response; 
        }

        [HttpPost("login")]
        public async Task<Result<LoginResponse>> Login(LoginViewModel viewModel)
        {
            var command = viewModel.Map<LoginCommand>();
            var response = await _mediator.Send(command);
           
            return response;
        }

        [HttpPost("verify")]
        public async Task<Result<bool>> VerifyAccount([FromQuery] string token, [FromQuery] string email)
        {

            var result = await _mediator.Send(new VerifyAccountCommand(email, token));
            return result;
        }
        
        [HttpPost("ChangePassword")]
        public async Task<Result<bool>> ChangePassword(ChangePasswordViewModel viewModel)
        {
            var command = viewModel.Map<ChangePasswordCommand>();
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPost("forgot-password")]
        public async Task<Result<bool>> ForgotPassword(ForgotPasswordViewModel viewModel)
        {
            var command = viewModel.Map<ForgotPasswordCommand>(); 

            var response = await _mediator.Send(command);

            return response;
        }

        [HttpPost("reset-password")]
        public async Task<Result<bool>> ResetPassword([FromQuery] string email,[FromQuery] string resetCode, string newPassword)
        {           
            var response = await _mediator.Send(new ResetPasswordCommand(email, resetCode, newPassword));

            return response;
        }
    }
}