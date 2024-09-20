using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;

namespace ProjectManagementSystem.CQRS.Projects.Query
{
    public record GetProjectCountQuery(SpecParams SpecParams) : IRequest<Result<int>>;

    public class GetProjectCountQueryHandler : IRequestHandler<GetProjectCountQuery, Result<int>>
{
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(GetProjectCountQuery request, CancellationToken cancellationToken)
        {
            var projectSpec = new CountProjectWithSpec(request.SpecParams);
            var count = await _unitOfWork.Repository<Project>().GetCountWithSpecAsync(projectSpec);

            return Result.Success(count);
        }
    }
}
