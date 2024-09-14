using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Commands.Orchestrators;

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
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
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
        public async Task<ActionResult<ChangePasswordResultDto>> ChangePassword([FromQuery] ChangePasswordCommand command)
        {
            var result = await _mediator.Send(command);
            return result;
        }

    }
}
