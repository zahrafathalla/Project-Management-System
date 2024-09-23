using MediatR;
using ProjectManagementSystem.Abstractions;

namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public record GetUserStatisticsQuery(int userId):IRequest<Result<UserReportToReturnDto>>;

    public class UserReportToReturnDto()
    {
        public int TotalNotActiveUser { get; set;}
        public decimal ActiveUserPercentage { get; set; }
    }

    public class GetUserStatisticsQueryHandler : IRequestHandler<GetUserStatisticsQuery, Result<UserReportToReturnDto>>
    {
        private readonly IMediator _mediator;

        public GetUserStatisticsQueryHandler(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<Result<UserReportToReturnDto>> Handle(GetUserStatisticsQuery request, CancellationToken cancellationToken)
        {
            var notActiveUsers = await _mediator.Send(new GetTotalNotActiveUserQuery(request.userId));

            var activeUsers = await _mediator.Send(new GetActiveUserPercentageQuery(request.userId));

            var report = new UserReportToReturnDto()
            {
                TotalNotActiveUser = notActiveUsers.Data,
                ActiveUserPercentage = activeUsers.Data,
            };

            return Result.Success(report);
        }  
    }
}
 
