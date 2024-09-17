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
        public async Task<Result<LoginResponse>> Login([FromBody] LoginViewModel viewModel)
        {
            var command = viewModel.Map<LoginCommand>();
            var response = await _mediator.Send(command);
           
            return response;
        }

        [HttpPost("verify")]
        public async Task<Result<bool>> VerifyAccount([FromBody] VerifyAccountViewModel viewModel)
        {
            var command = viewModel.Map<VerifyAccountCommand>(); 
            var result = await _mediator.Send(command);
            return result;
        }
        
        [HttpPost("ChangePassword")]
        public async Task<Result<bool>> ChangePassword([FromBody] ChangePasswordViewModel viewModel)
        {
            var command = viewModel.Map<ChangePasswordCommand>();
            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPost("forgot-password")]
        public async Task<Result<bool>> ForgotPassword([FromBody] ForgotPasswordViewModel viewModel)
        {
            var command = viewModel.Map<ForgotPasswordCommand>(); 

            var response = await _mediator.Send(command);

            return response;
        }

        [HttpPost("reset-password")]
        public async Task<Result<bool>> ResetPassword([FromBody] ResetPasswordViewModel viewModel)
        {
            var command = viewModel.Map<ResetPasswordCommand>();
            
            var response = await _mediator.Send(command);

            return response;
        }
    }
}