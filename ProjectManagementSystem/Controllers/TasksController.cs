using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.CQRS.Task.Command;
using ProjectManagementSystem.CQRS.Task.Query;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Specification;
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

        var response = await _mediator.Send(command);

        return response;
    }

    [HttpPost("AssignTask")]
    public async Task<Result> AssignTask([FromBody] AssignTaskViewModel viewModel)
    {
        var command = viewModel.Map<AssignTaskCommand>();

        var response = await _mediator.Send(command);
        return response;
    }


    [HttpGet("List-Tasks")]
    public async Task<Result<Pagination<TaskToReturnDto>>> GetAllTasks([FromQuery] SpecParams spec)
    {
        var result = await _mediator.Send(new GetTasksQuery(spec));
        if (!result.IsSuccess)
        {
            return Result.Failure<Pagination<TaskToReturnDto>>(result.Error);
        }

        var TasksCount = await _mediator.Send(new GetTaskCountQuery(spec));
        var paginationResult = new Pagination<TaskToReturnDto>(spec.PageSize, spec.PageIndex, TasksCount.Data, result.Data);

        return Result.Success(paginationResult);
    }
}
