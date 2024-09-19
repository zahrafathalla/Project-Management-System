using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Command;

public record UpdateProjectCommand(int ProjectId, string Title, string Description) : IRequest<Result<bool>>;

public class UpdateProjectHandler : IRequestHandler<UpdateProjectCommand, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;

    public UpdateProjectHandler(IUnitOfWork unitOfWork,IMediator mediator)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
    }

    public async Task<Result<bool>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        var projectresult = await _mediator.Send(new GetProjectByIdQuery(request.ProjectId));

        if (!projectresult.IsSuccess)
        {
            return Result.Failure<bool>(ProjectErrors.ProjectNotFound);
        }

        var project = projectresult.Data;
        project.Title = request.Title;
        project.Description = request.Description;

        _unitOfWork.Repository<Project>().Update(project);  
        await _unitOfWork.SaveChangesAsync();

        return Result.Success(true);
    }
}
