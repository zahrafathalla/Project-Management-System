using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Controllers;


public class UsersController : BaseController
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("List-User")]
    public async Task<Result<List<UserResponse>>> GetAllUsers()
    {
        var result = await _mediator.Send(new GetAllUsersQuery());
        return result;
    }

    [HttpGet("view-User/{projectId}")]
    public async Task<Result<UserResponse>> GetUserById(int UserId)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(UserId));

        return result; 
    }

}
