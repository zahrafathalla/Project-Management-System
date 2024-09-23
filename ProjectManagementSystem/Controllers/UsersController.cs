using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Abstractions.Consts;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Specification;

namespace ProjectManagementSystem.Controllers;

[Authorize(Roles = nameof(DefaultRoles.Admin))]
public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("List-Users")]
    public async Task<Result<Pagination<UserToReturnDto>>> GetAllProjects([FromQuery] SpecParams spec)
    {
        var result = await _mediator.Send(new GetAllUsersQuery(spec));
        if (!result.IsSuccess)
        {
            return Result.Failure<Pagination<UserToReturnDto>>(result.Error);
        }

        var UsertCount = await _mediator.Send(new GetUserCountQuery(spec));
        var paginationResult = new Pagination<UserToReturnDto>(spec.PageSize, spec.PageIndex, UsertCount.Data, result.Data);
        return Result.Success(paginationResult);
    }

    [HttpGet("view-User/{UserId}")]
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
