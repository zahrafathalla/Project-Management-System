﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Abstractions;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Errors;
using ProjectManagementSystem.Repository.Interface;


namespace ProjectManagementSystem.CQRS.Users.Queries
{
    public record GetUserByEmailQuery(string Email) : IRequest<Result<User>>;

    public class GetUserByEmailQueryHandler : IRequestHandler<GetUserByEmailQuery, Result<User>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserByEmailQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return Result.Failure<User>(UserErrors.InvalidEmail);
            }

            var user =  _unitOfWork.Repository<User>().GetWithInclude(u => u.Email == request.Email).Include(u => u.UserRoles).ThenInclude(r=>r.Role).FirstOrDefault();
            if (user == null)
            {
                return Result.Failure<User>(UserErrors.UserNotFound);
            }

            return Result.Success(user);
        }
    }
}

