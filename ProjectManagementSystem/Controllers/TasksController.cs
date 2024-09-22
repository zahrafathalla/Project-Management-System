using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Command;
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

    [HttpPost("Create-Task")]
    public async Task<Result<int>> CreateTask([FromBody] CreateTaskViewModel viewModel)
    {
       // var command = viewModel.Map<CreateTaskCommand>();

        var response = await _mediator.Send(new CreateTaskCommand(viewModel.Title, viewModel.ProjectId, viewModel.AssignedToUserId) );

        return response;
    }

    [HttpPost("Assign-Task")]
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

    [HttpDelete("Delete-Task/{taskId}")]
    public async Task<Result<bool>> DeleteTask(int taskId)
    {
        var result = await _mediator.Send(new DeleteTaskCommand(taskId));

        return result;
    }

    [HttpGet("view-Task/{taskId}")]
    public async Task<Result<TaskToReturnDto>> GetTasktById(int taskId)
    {
        var result = await _mediator.Send(new GetTaskByIdQuery(taskId));
        if (result.Data == null)
            return Result.Failure<TaskToReturnDto>(result.Error);

        var mappedTask = result.Data.Map<TaskToReturnDto>();
        return Result.Success(mappedTask);
    }

    [HttpPut("Update-Task/{taskId}")]
    public async Task<Result<bool>> UpdateProject(int taskId, UpdateTaskViewModel viewModel)
    {
        var result = await _mediator.Send(new UpdateTaskCommand(taskId, viewModel.Title));
        return result;
    }
}
