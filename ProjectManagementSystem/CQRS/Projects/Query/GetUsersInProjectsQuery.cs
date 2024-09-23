using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Repository.Specification.UserProjectSpecification;

namespace ProjectManagementSystem.CQRS.Projects.Query
{
    public record GetUsersInProjectsQuery(int UserId) : IRequest<Result<IEnumerable<User>>>;

    public class GetUsersInProjectsQueryHandler : IRequestHandler<GetUsersInProjectsQuery, Result<IEnumerable<User>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersInProjectsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<IEnumerable<User>>> Handle(GetUsersInProjectsQuery request, CancellationToken cancellationToken)
        {

            var spec = new UserProjectWithUserSpec(request.UserId);
            var userProjects = await _unitOfWork.Repository<UserProject>()
                                     .GetAllWithSpecAsync(spec);

            if (!userProjects.Any())
            {
                return Result.Success(Enumerable.Empty<User>());
            }

            var projectIds = userProjects.Select(up => up.ProjectId).Distinct().ToList();

            var specWithProjectIds = new UserProjectWithUserSpec(projectIds);

            var usersInProjects = await _unitOfWork.Repository<UserProject>()
                                      .GetAllWithSpecAsync(specWithProjectIds);
            var uniqueUsers = usersInProjects
                .Select(up => up.User)
                .Where(user => user != null)  
                .Distinct() 
                .ToList();

          
            return Result.Success((IEnumerable<User>) uniqueUsers);
        }
    }

}
