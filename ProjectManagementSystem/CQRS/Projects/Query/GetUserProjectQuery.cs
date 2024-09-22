using MediatR;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Query
{
    public record GetUserProjectQuery(int UserId, int ProjectId) : IRequest<UserProject?>;

    public class GetUserProjectQueryHandler : IRequestHandler<GetUserProjectQuery, UserProject?>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProjectQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProject?> Handle(GetUserProjectQuery request, CancellationToken cancellationToken)
        {
            var userProject =(await _unitOfWork.Repository<UserProject>()
                .GetAsync(up => up.UserId == request.UserId && up.ProjectId == request.ProjectId)).FirstOrDefault();
            
            return userProject;

        }
    }

}
