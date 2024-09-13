using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.CQRS.Users.Commands;

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
            return Ok(response.GetResult());
        }
        [HttpPost("CreateAccount")]
        public async Task<ActionResult<CreateAccountToReturnDto>> CreateAccount([FromBody] CreateAccountCommand command)
        {
            var response = await _mediator.Send(command);
            return Ok(response);
        }
    }
}
