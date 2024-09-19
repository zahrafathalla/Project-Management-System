using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Task.Command;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.ViewModel;

namespace ProjectManagementSystem.Controllers;

public class TasksController : BaseController
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("CreateTask")]
    public async Task<Result<int>> CreateTask([FromBody] CreateTaskViewModel viewModel)
    {
        var command = viewModel.Map<CreateTaskCommand>();

        var respons = await _mediator.Send(command);

        return respons;
    }
}
