using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Command;

public record DeleteProjectCommand(int ProjectId): IRequest<Result<bool>>;

public class DeleteProjectHandler : IRequestHandler<DeleteProjectCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public DeleteProjectHandler(IUnitOfWork unitOfWork, IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }
    public async Task<Result<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var projectresult = await _mediator.Send(new GetProjectByIdQuery(request.ProjectId));

        if (!projectresult.IsSuccess)
        {
            return Result.Failure<bool>(ProjectErrors.ProjectNotFound);
        }

        _unitOfWork.Repository<Project>().DeleteById(request.ProjectId);
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}
