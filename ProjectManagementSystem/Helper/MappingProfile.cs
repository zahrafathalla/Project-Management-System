using AutoMapper;
using ProjectManagementSystem.CQRS.Projects.Command;
using ProjectManagementSystem.CQRS.Projects.Command.Orchestrator;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.CQRS.Users.Commands.Orchestrators;
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

            CreateMap<LoginViewModel, LoginCommand>();
            CreateMap<ChangePasswordViewModel, ChangePasswordCommand>();
            CreateMap<ForgotPasswordViewModel, ForgotPasswordCommand>();
            CreateMap<VerifyAccountViewModel, VerifyAccountCommand>();
            CreateMap<ResetPasswordViewModel, ResetPasswordCommand>();
            CreateMap<CreateAccountViewModel, CreateAccountViewModel>();

            CreateMap<AddRoleToUserViewModel, AddRoleToUserCommand>();
            CreateMap<ResetPasswordViewModel, ResetPasswordCommand>();
            CreateMap<CreateAccountOrchestrator, CreateAccountCommand>();
            CreateMap<CreateAccountViewModel, CreateAccountOrchestrator>();

            CreateMap<AddProjectCommand, Project>()
                 .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => ProjectStatus.Public));

            CreateMap<AddProjectViewModel, AddProjectOrchestrator>();
        }
    }
}
