using AutoMapper;
using ProjectManagementSystem.CQRS.Users.Commands;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Helper
{
    public  class MappingProfile :Profile
    {
        public MappingProfile()
        {
            CreateMap<User, CreateAccountToReturnDto>();
            CreateMap<User, LoginResponse>();

        }
    }
}
