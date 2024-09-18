using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;

namespace ProjectManagementSystem.CQRS.Projects.Command
{
    public class AssignUsersToProjectCommand : IRequest<Result>
    {
        public int ProjectId { get; set; }
        public int CreatorUserId { get; set; }
        public List<int> UserIds { get; set; } = new List<int>();
    }

    public class AssignUsersToProjectCommandHandler : IRequestHandler<AssignUsersToProjectCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AssignUsersToProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(AssignUsersToProjectCommand request, CancellationToken cancellationToken)
        {
            var creatorUserProject = new UserProject
            {
                UserId = request.CreatorUserId,
                ProjectId = request.ProjectId,
                IsCreator = true
            };

            await _unitOfWork.Repository<UserProject>().AddAsync(creatorUserProject);


            foreach (var userId in request.UserIds.Where(id => id != request.CreatorUserId))
            {
                var userProject = new UserProject
                {
                    UserId = userId,
                    ProjectId = request.ProjectId,
                    IsCreator = false
                };

                await _unitOfWork.Repository<UserProject>().AddAsync(userProject);
            }

            await _unitOfWork.SaveChangesAsync();


            return Result.Success();
        }
    }
}
