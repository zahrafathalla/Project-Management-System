using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.TaskSpecifications
{
    public class TaskSpec : BaseSpecification<WorkTask>
    {
        public TaskSpec(SpecParams spec)
        {
            Includes.Add(p => p.Include(p => p.AssignedToUser)!);
            Includes.Add(p => p.Include(p => p.Project)!);

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
