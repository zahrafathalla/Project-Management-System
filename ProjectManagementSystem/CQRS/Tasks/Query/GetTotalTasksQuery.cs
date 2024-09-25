using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Tasks.Query
{
    public record GetTotalTasksQuery(int userId) :IRequest<Result<int>>;

    public class GetTotalTasksQueryHandler : IRequestHandler<GetTotalTasksQuery, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalTasksQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(GetTotalTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _unitOfWork.Repository<WorkTask>()
                                         .GetAsync(t => t.AssignedToUserId == request.userId);
            
            var totalTasks = tasks.Count();

            return Result.Success(totalTasks);
        }
    }
}
