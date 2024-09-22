using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;

namespace ProjectManagementSystem.CQRS.Projects.Query
{
    public record GetTotalProjectsQuery(int userId) : IRequest<Result<int>>;

    public class GetTotalProjectsQueryHandler : IRequestHandler<GetTotalProjectsQuery, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTotalProjectsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(GetTotalProjectsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProjectWithUserSpec(request.userId);

            var projects = await _unitOfWork.Repository<Project>().GetAllWithSpecAsync(spec);                                       

            var totalProjects = projects.Count();

            return Result.Success(totalProjects);
        }
    }
}
