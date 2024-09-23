using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Task.Query;
using ProjectManagementSystem.DTO;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.Controllers
{

    public class ReportingController : BaseController
    {
        private readonly IMediator _mediator;

        public ReportingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("Task-Stats")]
        [Authorize]
        public async Task<Result<TaskReportingToReturnDto>> GetTaskStats()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var result = await _mediator.Send(new GetTaskStatisticsQuery(userId));

            return result;
        }


        [HttpGet("Task-Board/{projectId}")]
        [Authorize]
        public async Task<Result<TaskListToReturnDto>> GetTasksByProjectId(int projectId)
        {
            var query = new GetTasksByProjectIdQuery(projectId);

            var result = await _mediator.Send(query);

            var taskListToReturnDto = result.Data.Map<TaskListToReturnDto>();

            return Result.Success(taskListToReturnDto);
        }
    }
}