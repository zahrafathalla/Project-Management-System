using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Task.Query;

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

    }
}
