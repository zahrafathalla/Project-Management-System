using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Task.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Task.Command
{
    public record DeleteTaskCommand(int taskId) : IRequest<Result<bool>>;

    public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public DeleteTaskCommandHandler(
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }
        public async Task<Result<bool>> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
        {
            var taskresult = await _mediator.Send(new GetTaskByIdQuery(request.taskId));

            if (!taskresult.IsSuccess)
            {
                return Result.Failure<bool>(TaskErrors.TaskNotFound);
            }

            _unitOfWork.Repository<WorkTask>().DeleteById(request.taskId);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }
}
