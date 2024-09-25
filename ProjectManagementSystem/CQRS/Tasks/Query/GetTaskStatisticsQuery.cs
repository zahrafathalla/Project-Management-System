using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;

namespace ProjectManagementSystem.CQRS.Tasks.Query
{
    public record GetTaskStatisticsQuery(int userId) : IRequest<Result<TaskReportToReturnDto>>;

    public class TaskReportToReturnDto
    {
        public decimal TotalProgress { get; set; }
        public int TotalTasks { get; set; }
        public int TotalProjects { get; set; }
    }
    public class GetTaskStatisticsQueryHandler : IRequestHandler<GetTaskStatisticsQuery, Result<TaskReportToReturnDto>>
    {

        private readonly IMediator _mediator;

        public GetTaskStatisticsQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<TaskReportToReturnDto>> Handle(GetTaskStatisticsQuery request, CancellationToken cancellationToken)
        {
            var totalTasks = await _mediator.Send(new GetTotalTasksQuery(request.userId));
            
            var totalProjects = await _mediator.Send(new GetTotalProjectsQuery(request.userId));

            var progress = await _mediator.Send(new GetTasksProgressQuery(request.userId));

            var report = new TaskReportToReturnDto
            {
                TotalProgress = progress.Data,
                TotalProjects = totalProjects.Data,
                TotalTasks = totalTasks.Data
            };

            return Result.Success(report);
        }
    }

}
