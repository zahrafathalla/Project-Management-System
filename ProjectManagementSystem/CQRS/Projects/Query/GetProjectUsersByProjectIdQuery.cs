using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification.ProjectSpecifications;
using ProjectManagementSystem.Repository.Specification.UserProjectSpecification;

namespace ProjectManagementSystem.CQRS.Projects.Query
{
    public record GetProjectUsersByProjectIdQuery(int ProjectId) :IRequest<Result<IEnumerable<User>>>;

    public class GetProjectUsersByProjectIdQueryHandler : IRequestHandler<GetProjectUsersByProjectIdQuery, Result<IEnumerable<User>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetProjectUsersByProjectIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<User>>> Handle(GetProjectUsersByProjectIdQuery request, CancellationToken cancellationToken)
        {

            var usersInProjectSpec = new UserPrpojectByProjectIdWithUserSpec(request.ProjectId);
            var userProjects = await _unitOfWork.Repository<UserProject>().GetAllWithSpecAsync(usersInProjectSpec);

            if (!userProjects.Any())
            {
                return Result.Success(Enumerable.Empty<User>());
            }

            var uniqueUsers = userProjects
                .Select(up => up.User)
                .Where(user => user != null)
                .Distinct()
                .ToList();

            return Result.Success(uniqueUsers as IEnumerable<User>);
        }
    }



}
