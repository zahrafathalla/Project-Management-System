using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Users.Commands;

namespace ProjectManagementSystem.Controllers;

public class RolesController : BaseController
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("AddRoleToUser")]
    public async Task<ActionResult<bool>> Login([FromBody] AddRoleToUserCommand command)
    {
        var response = await _mediator.Send(command);
        return Ok(response);
    }
}
