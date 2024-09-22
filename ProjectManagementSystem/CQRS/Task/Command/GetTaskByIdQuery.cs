using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Command
{
    public record GetTaskByIdQuery(int taskId) : IRequest<Result<WorkTask>>;

    public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, Result<WorkTask>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTaskByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<WorkTask>> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var task = (await _unitOfWork.Repository<WorkTask>().GetAsync(p => p.Id == request.taskId && !p.IsDeleted)).FirstOrDefault();
            if (task == null)
            {
                return Result.Failure<WorkTask>(TaskErrors.TaskNotFound);
            }

            return Result.Success(task);
        }
    }
}
