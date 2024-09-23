using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Command
{
    public record ChangeProjectStatusCommand( int ProjectId, ProjectStatus NewStatus) : IRequest<Result<bool>>;
    public class ChangeProjectStatusCommandHandler : IRequestHandler<ChangeProjectStatusCommand, Result<bool>>
    {

        private readonly IMediator _mediator;
        private readonly IUnitOfWork _unitOfWork;

        public ChangeProjectStatusCommandHandler(
            IMediator mediator, IUnitOfWork unitOfWork)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(ChangeProjectStatusCommand request, CancellationToken cancellationToken)
        {

            var projectResult = await _mediator.Send(new GetProjectByIdQuery(request.ProjectId));

            if (projectResult == null)
            {
                return Result.Failure<bool>(ProjectErrors.ProjectNotFound);
            }

            var project = projectResult.Data;
            project.Status = request.NewStatus;

            _unitOfWork.Repository<Project>().Update(project);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(true);
        }
    }

}
