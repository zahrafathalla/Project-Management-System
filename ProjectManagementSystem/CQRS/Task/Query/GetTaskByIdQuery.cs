using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification.TaskSpecifications;

namespace ProjectManagementSystem.CQRS.Task.Query
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
            var spec = new TaskWithProjectSpec(request.taskId);
            var task = await _unitOfWork.Repository<WorkTask>().GetByIdWithSpecAsync(spec);

            if (task == null)
            {
                return Result.Failure<WorkTask>(TaskErrors.TaskNotFound);
            }
            return Result.Success(task);
        }
    }
}
