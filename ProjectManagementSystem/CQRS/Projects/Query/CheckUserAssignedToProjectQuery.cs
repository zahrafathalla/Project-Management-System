using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Query;

public record CheckUserAssignedToProjectQuery(int UserId, int ProjectId) : IRequest<Result<bool>>;

public class CheckUserAssignedToProjectQueryHandler : IRequestHandler<CheckUserAssignedToProjectQuery, Result<bool>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CheckUserAssignedToProjectQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(CheckUserAssignedToProjectQuery request, CancellationToken cancellationToken)
    {
        var userProject = await _unitOfWork.Repository<UserProject>()
            .GetAsync(up => up.UserId == request.UserId && up.ProjectId == request.ProjectId);

        return Result.Success(userProject.Any());
    }
}
