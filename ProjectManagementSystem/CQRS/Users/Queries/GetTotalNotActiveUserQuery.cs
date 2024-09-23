using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public record GetTotalNotActiveUserQuery(int UserId) : IRequest<Result<int>>;

    public class GetTotalNotActiveUserQueryHandler : IRequestHandler<GetTotalNotActiveUserQuery, Result<int>>
    {
        private readonly IMediator _mediator;

        public GetTotalNotActiveUserQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<int>> Handle(GetTotalNotActiveUserQuery request, CancellationToken cancellationToken)
        {

            var usersInProjects = await _mediator.Send(new GetUsersInProjectsQuery(request.UserId));

            if (!usersInProjects.Data.Any())
            {
                return Result.Success(0);
            }

            var inactiveUsersCount = usersInProjects.Data.Count(up => up.Status == UserStatus.NotActive);

            return Result.Success(inactiveUsersCount);
        }
    }
}
