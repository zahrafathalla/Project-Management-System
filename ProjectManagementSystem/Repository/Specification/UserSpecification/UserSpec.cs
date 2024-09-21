using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.UserSpecification;

public class UserSpec : BaseSpecification<User>
{
    public UserSpec(SpecParams spec)
    {
        if (!string.IsNullOrEmpty(spec.Search))
        {
            Criteria = p => p.UserName.ToLower().Contains(spec.Search.ToLower());
        }

        ApplyPagination(spec.PageSize * (spec.PageIndex - 1), spec.PageSize);
    }
}
