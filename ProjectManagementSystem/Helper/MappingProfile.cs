using AutoMapper;
using ProjectManagementSystem.CQRS.Roles.Command;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.ViewModel;

namespace ProjectManagementSystem.Helper
{
    public  class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CreateAccountToReturnDto>();
            CreateMap<User, LoginResponse>();

            CreateMap<LoginViewModel, LoginCommand>();
            CreateMap<ChangePasswordViewModel, ChangePasswordCommand>();

            CreateMap<AddRoleToUserViewModel, AddRoleToUserCommand>();


        }
    }
}
