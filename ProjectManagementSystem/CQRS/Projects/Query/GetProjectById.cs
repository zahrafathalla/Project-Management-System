using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Queries;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Query;

public record GetProjectByIdQuery(int ProjectId) : IRequest<Result<Project>>;

public class GetProjectByIdQueryHandler : IRequestHandler<GetProjectByIdQuery, Result<Project>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetProjectByIdQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task<Result<Project>> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = (await _unitOfWork.Repository<Project>().GetAsync(p => p.Id == request.ProjectId && !p.IsDeleted)).FirstOrDefault();
        if (project == null)
        {
            return Result.Failure<Project>(ProjectErrors.ProjectNotFound);
        }

        return Result.Success(project);
    }
}

