using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.ViewModel;

namespace ProjectManagementSystem.Controllers;

public class RolesController : BaseController
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("AddRoleToUser")]
    public async Task<Result<bool>> AddRoleToUser([FromBody] AddRoleToUserViewModel viewModel)
    {
        var response = await _mediator.Send(new AddRoleToUserCommand(viewModel.UserId, viewModel.RoleName));
        return response;
    }

    [HttpPost("RemoveRoleFromUser")]
    public async Task<Result<bool>> RemoveRoleFromUser([FromBody] int userId, int roleId)
    {
        var response = await _mediator.Send(new RemoveRoleFromUserCommand(userId, roleId));
        return response;
    }
}
