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

        [HttpPost("login")]
        public async Task<Result<LoginResponse>> Login([FromBody] LoginViewModel viewModel)
        {
            var command = viewModel.Map<LoginCommand>();
            var response = await _mediator.Send(command);
           
            return response;
        }
  

        [HttpPost("CreateAccount")]
        public async Task<ActionResult<CreateAccountOrchestratorToReturnDto>> CreateAccount([FromBody] CreateAccountOrchestrator command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpPost("verify")]
        public async Task<ActionResult<bool>> VerifyAccount([FromQuery] string email, [FromQuery] string token)
        {
            var result = await _mediator.Send(new VerifyAccountCommand { Email = email, Token = token });
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
        public async Task<Result<bool>> ForgotPassword([FromBody] string email)
        {
            var command = new ForgotPasswordCommand
            {
                Email = email
            };

            var response = await _mediator.Send(command);
            return response;
        }

        [HttpPost("reset-password")]
        public async Task<Result<bool>> ResetPassword([FromQuery] string email , [FromQuery] string resetCode, [FromBody] string NewPassword)
        {
            var command = new ResetPasswordCommand
            {
                Email = email,
                Code = resetCode,
                NewPassword = NewPassword
            };

            var response = await _mediator.Send(command);
            return response;
        }
    }
}
