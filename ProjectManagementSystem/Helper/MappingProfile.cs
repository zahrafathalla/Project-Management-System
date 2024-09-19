﻿using AutoMapper;
using ProjectManagementSystem.CQRS.Projects.Command;
using ProjectManagementSystem.CQRS.Projects.Command.Orchestrator;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Task.Command;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Commands.Orchestrators;
using ProjectManagementSystem.CQRS.Users.Response;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Data.Entities.Enums;
using ProjectManagementSystem.ViewModel;

namespace ProjectManagementSystem.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAccountCommand, User>();
            CreateMap<User, LoginResponse>();
            CreateMap<User, UserResponse>();

            CreateMap<LoginViewModel, LoginCommand>();
            CreateMap<CreateAccountViewModel, CreateAccountOrchestrator>();
            CreateMap<ChangePasswordViewModel, ChangePasswordCommand>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordCommand>();
            CreateMap<VerifyAccountViewModel, VerifyAccountCommand>();
            CreateMap<ResetPasswordViewModel, ResetPasswordCommand>();
            CreateMap<CreateAccountViewModel, CreateAccountViewModel>();

            CreateMap<AddRoleToUserViewModel, AddRoleToUserCommand>();
            CreateMap<CreateAccountOrchestrator, CreateAccountCommand>();


            CreateMap<AddProjectCommand, Project>()
                 .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow));
            
            CreateMap<CreateTaskCommand, WorkTask>()
                 .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow));


            CreateMap<CreateTaskViewModel, CreateTaskCommand>();
            CreateMap<ChangeUserStatusCommand, User>()
              .ForMember(dest => dest.Status, opt => opt.MapFrom(src => Enum.Parse<UserStatus>(src.NewStatus, true)));
            ;

            CreateMap<UpdateProjectCommand, Project>();
            CreateMap<UpdateProjectViewModel, UpdateProjectCommand>();
            CreateMap<DeleteProjectViewModel, DeleteProjectCommand>();

            CreateMap<AddProjectViewModel, AddProjectOrchestrator>();
        }
    }
}
