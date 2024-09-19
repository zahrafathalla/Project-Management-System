using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.Controllers;


public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("List-Users")]
    public async Task<Result<List<UserResponse>>> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return result;
    }

    [HttpGet("view-User/{projectId}")]
    public async Task<Result<UserResponse>> GetUserById(int UserId)
    {
        var userResult = await _mediator.Send(new GetUserByIdQuery(UserId));
        if (!userResult.IsSuccess)
        {
            return Result.Failure<UserResponse>(userResult.Error);
        }
        var user = userResult.Data;
        var userResponse = user.Map<UserResponse>();

        return Result.Success(userResponse);
    }

    [HttpPut("ChangeUserStatus")]
    public async Task<Result<bool>> ChangeUserStatus([FromBody] ChangeUserStatusCommand command)
    {
        var result = await _mediator.Send(new ChangeUserStatusCommand(command.UserId, command.NewStatus));
        if (!result.IsSuccess)
        {
            Result.Failure<bool>(result.Error);
        }

        return Result.Success(true);
    }
}
