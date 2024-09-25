using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data.Entities;

namespace ProjectManagementSystem.Repository.Specification.TaskSpecifications
{
    public class TaskWithProjectSpec :BaseSpecification<WorkTask>
    {
        public TaskWithProjectSpec(int taskId)
            :base(t=>t.Id==taskId)
        {
            Includes.Add(t => t.Include(t => t.Project));
            Includes.Add(t => t.Include(t => t.AssignedToUser));

        }
    }
}
