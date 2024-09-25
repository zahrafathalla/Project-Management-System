using MediatR;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Query
{
    public record GetUsersInProjectByProjectIdQuery(int UserId, int ProjectId) : IRequest<UserProject?>;

    public class GetUsersInProjectByIdQueryHandler : IRequestHandler<GetUsersInProjectByProjectIdQuery, UserProject?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUsersInProjectByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProject?> Handle(GetUsersInProjectByProjectIdQuery request, CancellationToken cancellationToken)
        {
            var userProject =(await _unitOfWork.Repository<UserProject>()
                .GetAsync(up => up.UserId == request.UserId && up.ProjectId == request.ProjectId)).FirstOrDefault();
            
            return userProject;

        }
    }

}
