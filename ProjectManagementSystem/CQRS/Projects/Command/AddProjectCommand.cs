﻿using MediatR;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Helper;

namespace ProjectManagementSystem.CQRS.Projects.Command
{
    public class AddProjectCommand : IRequest<Result<int>>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
    }

    public class AddProjectCommandHandler : IRequestHandler<AddProjectCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddProjectCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(AddProjectCommand request, CancellationToken cancellationToken)
        {
            var newProject = request.Map<Project>();

            await _unitOfWork.Repository<Project>().AddAsync(newProject);
            await _unitOfWork.SaveChangesAsync();

            return Result.Success(newProject.Id);
        }
    }
}
