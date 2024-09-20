using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;

namespace ProjectManagementSystem.CQRS.Projects.Query
{

    public record GetProjectsQuery(SpecParams SpecParams) : IRequest<Result<IEnumerable<ProjectToReturnDto>>>;
    public class ProjectToReturnDto
    {
        public string Title { get; set; }
        public string Status { get; set; }
        public int NumUsers { get; set; }
        public int NumTasks { get; set; }
        public DateTime DateCreated { get; set; }
    }
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, Result<IEnumerable<ProjectToReturnDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<ProjectToReturnDto>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            var spec = new ProjectSpec(request.SpecParams);

            var projects = await _unitOfWork.Repository<Project>().GetAllWithSpecAsync(spec);

            var mappedProject = projects.Map<IEnumerable<ProjectToReturnDto>>();

            return Result.Success(mappedProject);
        }
    }
}
