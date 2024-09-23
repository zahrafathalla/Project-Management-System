using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Query
{
    public record GetTasksProgressQuery(int UserId) : IRequest<Result<decimal>>;

    public class GetTotalProgressQueryHandler : IRequestHandler<GetTasksProgressQuery, Result<decimal>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalProgressQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<decimal>> Handle(GetTasksProgressQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _unitOfWork.Repository<WorkTask>()
                                          .GetAsync(t => t.AssignedToUserId == request.UserId);

            if (!tasks.Any())
            {
                return Result.Success(0m);
            }

            var totalTasks = tasks.Count();
            var completedTasks = tasks.Count(t => t.Status == WorkTaskStatus.Done);

            decimal progress = Math.Round((decimal)completedTasks / totalTasks * 100, 2);

            return Result.Success(progress);
        }
    }
}
