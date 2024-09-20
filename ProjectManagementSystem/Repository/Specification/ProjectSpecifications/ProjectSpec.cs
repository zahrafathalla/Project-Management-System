using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.ProjectSpecifications
{
    public class ProjectSpec :BaseSpecification<Project>
    {
        public ProjectSpec(SpecParams spec)
        {
            Includes.Add(p => p.Include(p => p.Tasks));
            Includes.Add(p => p.Include(p => p.UserProjects).ThenInclude(up=> up.User));

            if (!string.IsNullOrEmpty(spec.Search))
            {
                Criteria = p => p.Title.ToLower().Contains(spec.Search.ToLower());
            }

            if (!string.IsNullOrEmpty(spec.Sort))
            {
                switch (spec.Sort.ToLower())
                {
                    case "title":
                        AddOrderBy(p => p.Title);
                        break;
                    default:
                        AddOrderBy(p => p.DateCreated); 
                        break;
                }
            }

            ApplyPagination(spec.PageSize * (spec.PageIndex - 1), spec.PageSize);

        }
    }
}
