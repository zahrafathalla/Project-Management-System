using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Projects.Query;
using ProjectManagementSystem.Data.Entities.Enums;

namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public record GetActiveUserPercentageQuery(int UserId) : IRequest<Result<decimal>>;

    public class GetActiveUserPercentageQueryHandler : IRequestHandler<GetActiveUserPercentageQuery, Result<decimal>>
    {
        private readonly IMediator _mediator;

        public GetActiveUserPercentageQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result<decimal>> Handle(GetActiveUserPercentageQuery request, CancellationToken cancellationToken)
        {
            
            var usersInProjects = await _mediator.Send(new GetUsersInProjectsQuery(request.UserId), cancellationToken);

            if (!usersInProjects.Data.Any())
            {
                return Result.Success(0m); 
            }

            var totalUsers = usersInProjects.Data.Count();

            var activeUsersCount = usersInProjects.Data.Count(up => up.Status == UserStatus.Active);


            decimal activeUserPercentage = Math.Round((decimal)activeUsersCount / totalUsers * 100, 2);
                
            return Result.Success(activeUserPercentage);
        }
    }

}
