using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.UserSpecification;

public class CountUserWithSpec : BaseSpecification<User>
{
    public CountUserWithSpec(SpecParams specParams)
       : base(p => !p.IsDeleted)
    {

    }
}